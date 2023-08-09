using System.Text;
using Azure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using TheResistanceOnline.BusinessLogic.BotObservers;
using TheResistanceOnline.BusinessLogic.Emails;
using TheResistanceOnline.BusinessLogic.Games;
using TheResistanceOnline.BusinessLogic.Games.DbQueries;
using TheResistanceOnline.BusinessLogic.PlayerStatistics;
using TheResistanceOnline.BusinessLogic.Settings;
using TheResistanceOnline.BusinessLogic.Settings.Models;
using TheResistanceOnline.BusinessLogic.Timers;
using TheResistanceOnline.BusinessLogic.Users;
using TheResistanceOnline.BusinessLogic.Users.DbQueries;
using TheResistanceOnline.BusinessLogic.UserSettings;
using TheResistanceOnline.Data;
using TheResistanceOnline.Data.Entities.UserEntities;
using TheResistanceOnline.Data.Queries.Games;
using TheResistanceOnline.Data.Queries.UserQueries;
using TheResistanceOnline.Infrastructure.Data;
using TheResistanceOnline.Infrastructure.Data.Interceptors.CoreInterceptors;

namespace TheResistanceOnline.SocketServer.DI;

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

        services.AddAuthentication(x =>
                                   {
                                       x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                                       x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                                   })
                .AddJwtBearer(x =>
                              {
                                  x.RequireHttpsMetadata = true;
                                  x.SaveToken = true;
                                  x.TokenValidationParameters = new TokenValidationParameters
                                                                {
                                                                    ValidateIssuerSigningKey = true,
                                                                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                                                                                                                    .GetBytes(GetSettings(serviceProvider).JWTSecurityKey)),
                                                                    ValidateIssuer = false,
                                                                    ValidateAudience = false
                                                                };
                                  x.Events = new JwtBearerEvents
                                             {
                                                 // So on every message the server receives get the token from the this.options
                                                 // which has the token
                                                 OnMessageReceived = context =>
                                                                     {
                                                                         var accessToken = context.Request.Query["access_token"];

                                                                         // If the request is for our hub...
                                                                         var path = context.HttpContext.Request.Path;
                                                                         // shane if you running into problems with the SignalR auth its cause of this stupid path segment
                                                                         if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/theresistancehub"))
                                                                         {
                                                                             // Read the token out of the query string
                                                                             context.Token = accessToken;
                                                                         }

                                                                         return Task.CompletedTask;
                                                                     }
                                             };
                              });
    }

    public static void AddContext(this IServiceCollection services)
    {
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
        services.AddScoped<IGameService, GameService>();
        services.AddScoped<INaiveBayesClassifierService, NaiveBayesClassifierService>();
        services.AddScoped<IPlayerStatisticService, PlayerStatisticService>();
        //ToDo probs not needed
        services.AddScoped<ITimerService, TimerService>();

        // Queries
        services.AddTransient<IUserByNameOrEmailDbQuery, UserByNameOrEmailDbQuery>();
        services.AddTransient<IUserDbQuery, UserDbQuery>();
        services.AddTransient<IAllGamePlayerValuesDbQuery, AllGamePlayerValuesDbQuery>();

        // Interceptors
        services.AddSingleton<UpdateAuditableEntitiesInterceptor>();

        // Mapping Profiles
        services.AddAutoMapper(typeof(UserMappingProfile));
        services.AddAutoMapper(typeof(GameMappingProfile));
        services.AddAutoMapper(typeof(PlayerStatisticMappingProfile));
        // Identities
        services.AddIdentity<User, IdentityRole>(options =>
                                                 {
                                                     options.User.RequireUniqueEmail = true;
                                                     options.SignIn.RequireConfirmedEmail = true;
                                                     options.Password.RequireNonAlphanumeric = false;
                                                     options.Password.RequireDigit = true;
                                                     options.Lockout.AllowedForNewUsers = true;
                                                     options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(7);
                                                     options.Lockout.MaxFailedAccessAttempts = 5;
                                                 })
                .AddEntityFrameworkStores<Context>()
                .AddDefaultTokenProviders();
    }

    #endregion
}
