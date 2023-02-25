using System.Text;
using Azure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using TheResistanceOnline.BusinessLogic.Emails;
using TheResistanceOnline.BusinessLogic.Settings;
using TheResistanceOnline.BusinessLogic.Settings.Models;
using TheResistanceOnline.BusinessLogic.Users;
using TheResistanceOnline.BusinessLogic.Users.DbQueries;
using TheResistanceOnline.BusinessLogic.UserSettings;
using TheResistanceOnline.Data;
using TheResistanceOnline.Data.Users;
using TheResistanceOnline.Infrastructure.Data;
using TheResistanceOnline.Infrastructure.Data.Interceptors.CoreInterceptors;
using TheResistanceOnline.Infrastructure.Data.Queries.Users;

namespace TheResistanceOnline.Web.DI;

public static class DISetup
{
    #region Fields

    private static readonly string? _appConfiguration = Environment.GetEnvironmentVariable("AppConfigurationConnection");

    #endregion

    #region Private Methods

    private static Settings GetSettings(IServiceProvider services)
    {
        return services.GetRequiredService<IOptions<Settings>>().Value;
    }

    #endregion

    #region Public Methods

    public static void AddAppSettings(this IServiceCollection services, WebApplicationBuilder builder)
    {
#if DEBUG
        services.Configure<Settings>(builder.Configuration.GetSection("DevApp:AppSettings"));
#elif RELEASE
        if (string.IsNullOrEmpty(_appConfiguration))
        {
            throw new ArgumentNullException(_appConfiguration, "Missing AppConfigurationConnection");
        }

        builder.Configuration.AddAzureAppConfiguration(options =>
                                                       {
                                                           options.Connect(new Uri(_appConfiguration), new ManagedIdentityCredential())
                                                                  .ConfigureKeyVault(kv => { kv.SetCredential(new DefaultAzureCredential()); });
                                                           options.Select("*", "Prod");
                                                       });
        services.Configure<Settings>(builder.Configuration.GetSection("ProdApp:AppSettings"));
#endif
        var serviceProvider = services.BuildServiceProvider();

        var settings = GetSettings(serviceProvider);
        if (settings.GetType().GetProperties()
                    .Where(pi => pi.PropertyType == typeof(string))
                    .Select(pi => (string)pi.GetValue(settings)!)
                    .Any(string.IsNullOrEmpty))
        {
            throw new ArgumentNullException(nameof(settings), $"Empty Setting Value(s) {JsonConvert.SerializeObject(settings)}");
        }
    }

    public static void AddAuthenticationServices(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();

        services.AddAuthentication(opt =>
                                   {
                                       opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                                       opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                                   }).AddJwtBearer(options =>
                                                   {
                                                       options.TokenValidationParameters = new TokenValidationParameters
                                                                                           {
                                                                                               ValidateIssuer = true,
                                                                                               ValidateAudience = true,
                                                                                               ValidateLifetime = true,
                                                                                               ValidateIssuerSigningKey = true,
                                                                                               ValidIssuer = GetSettings(serviceProvider).JWTValidIssuer,
                                                                                               ValidAudience = GetSettings(serviceProvider).JWTValidAudience,
                                                                                               IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                                                                                                   .GetBytes(GetSettings(serviceProvider).JWTSecurityKey))
                                                                                           };
                                                   });
    }

    public static void AddContext(this IServiceCollection services)
    {
        // Database
        services.AddDbContext<Context>((sp, options) =>
                                       {
                                           var auditableInterceptor = sp.GetService<UpdateAuditableEntitiesInterceptor>();
                                           options.UseSqlServer(GetSettings(sp).ResistanceDbConnectionString)
                                                  .AddInterceptors(auditableInterceptor ?? throw new InvalidOperationException("auditableInterceptor cannot be null"));
                                       });
        services.AddScoped<IDataContext, DataContext>();
    }

    public static void AddServices(this IServiceCollection services)
    {
        // Services
        services.AddScoped<ISettingsService, SettingsService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserIdentityManager, UserIdentityManager>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IUserSettingsService, UserSettingsService>();

        // Queries
        services.AddTransient<IUserByNameOrEmailDbQuery, UserByNameOrEmailDbQuery>();
        services.AddTransient<IUserDbQuery, UserDbQuery>();

        // Interceptors
        services.AddSingleton<UpdateAuditableEntitiesInterceptor>();

        // Identities 
        // Reset passwords tokens last for one hour
        services.Configure<DataProtectionTokenProviderOptions>(opt =>
                                                                   opt.TokenLifespan = TimeSpan.FromHours(1));

        services.AddIdentity<User, IdentityRole>(options =>
                                                 {
                                                     options.User.RequireUniqueEmail = true;
                                                     options.SignIn.RequireConfirmedEmail = true;
                                                     options.Password.RequireNonAlphanumeric = false;
                                                     options.Password.RequireDigit = true;
                                                     options.Lockout.AllowedForNewUsers = true;
                                                     options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(7);
                                                     options.Lockout.MaxFailedAccessAttempts = 10;
                                                 })
                .AddEntityFrameworkStores<Context>()
                .AddDefaultTokenProviders();

        // Mapping Profiles
        services.AddAutoMapper(typeof(UserMappingProfile));
    }

    #endregion
}
