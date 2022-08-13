using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TheResistanceOnline.BusinessLogic.Emails;
using TheResistanceOnline.BusinessLogic.Users;
using TheResistanceOnline.Data.Users;
using TheResistanceOnline.Infrastructure.Data;

namespace TheResistanceOnline.Web.DI;

public static class DISetup
{
    #region Public Methods

    public static void AddAuthenticationServices(this IServiceCollection services, IConfigurationSection? jwtSettings)
    {
        if (jwtSettings == null)
        {
            throw new ArgumentNullException(nameof(jwtSettings));
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
                                                                                               ValidIssuer = jwtSettings["validIssuer"],
                                                                                               ValidAudience = jwtSettings["validAudience"],
                                                                                               IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                                                                                                   .GetBytes(jwtSettings.GetSection("securityKey").Value))
                                                                                           };
                                                   });
    }

    public static void AddContext(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<Context>(options => options.UseSqlServer(connectionString));
    }

    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserIdentityManager, UserIdentityManager>();
        services.AddScoped<IEmailService, EmailService>();

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
    }

    #endregion
}
