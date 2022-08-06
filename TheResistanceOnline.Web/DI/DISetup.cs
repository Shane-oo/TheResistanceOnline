using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using TheResistanceOnline.BusinessLogic.Users;

namespace TheResistanceOnline.Web.DI;

public static class DISetup
{
    #region Public Methods

    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserIdentityManager, UserIdentityManager>();
                                                                      
    }

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

    #endregion
}