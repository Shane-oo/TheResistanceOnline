using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TheResistanceOnline.BusinessLogic.DiscordServer;
using TheResistanceOnline.BusinessLogic.Emails;
using TheResistanceOnline.BusinessLogic.Users;
using TheResistanceOnline.BusinessLogic.Users.DbQueries;
using TheResistanceOnline.Data;
using TheResistanceOnline.Data.Users;
using TheResistanceOnline.Infrastructure.Data;
using TheResistanceOnline.Infrastructure.Data.Queries.Users;

namespace TheResistanceOnline.Web.DI;

public static class DISetup
{
    #region Fields

    private static readonly string? _connectionString = Environment.GetEnvironmentVariable("ResistanceDb");

    private static readonly string? _securityKey = Environment.GetEnvironmentVariable("SecurityKey");
    private static readonly string? _validAudience = Environment.GetEnvironmentVariable("ValidAudience");
    private static readonly string? _validIssuer = Environment.GetEnvironmentVariable("ValidIssuer");

    #endregion

    #region Public Methods

    public static void AddAuthenticationServices(this IServiceCollection services)
    {
        if (_validIssuer == null || _validAudience == null || _securityKey == null)
        {
            throw new NullReferenceException("JWT settings not found");
        }

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
                                                                                               ValidIssuer = _validIssuer,
                                                                                               ValidAudience = _validAudience,
                                                                                               IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                                                                                                   .GetBytes(_securityKey))
                                                                                           };
                                                   });
    }

    public static void AddContext(this IServiceCollection services)
    {
        if (_connectionString == null)
        {
            throw new NullReferenceException("Connection string not found");
        }

        services.AddDbContext<Context>(options => options.UseSqlServer(_connectionString));
        services.AddScoped<IDataContext, DataContext>();
    }

    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserIdentityManager, UserIdentityManager>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddTransient<IUserDbQuery, UserDbQuery>();
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
                                                     options.Lockout.MaxFailedAccessAttempts = 5;
                                                 })
                .AddEntityFrameworkStores<Context>()
                .AddDefaultTokenProviders();

        services.AddAutoMapper(typeof(UserMappingProfile));
        services.AddAutoMapper(typeof(DiscordServerMappingProfile));

    }

    #endregion
}
