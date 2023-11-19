using TheResistanceOnline.Core;

namespace TheResistanceOnline.Server;

public static class DependencyInjection
{
    #region Public Methods

    public static void AddOpenIddictIntrospection(this IServiceCollection services, AppSettings appSettings, IWebHostEnvironment environment)
    {
        services.AddOpenIddict()
                .AddValidation(o =>
                               {
                                   o.SetIssuer(appSettings.SocketServerSettings.Issuer);

                                   o.UseIntrospection()
                                    .SetClientId(appSettings.SocketServerSettings.ClientId)
                                    .SetClientSecret(appSettings.SocketServerSettings.ClientSecret);

                                   o.UseSystemNetHttp();
                                   o.UseAspNetCore();
                               });
    }

    #endregion
}
