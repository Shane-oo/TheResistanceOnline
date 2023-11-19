using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;
using TheResistanceOnline.Core;
using TheResistanceOnline.Data;
using TheResistanceOnline.Data.Entities;
using TheResistanceOnline.Data.Interceptors;
using TheResistanceOnline.Users.Users;

namespace TheResistanceOnline.Web;

public static class DependencyInjection
{
    #region Private Methods

    private static X509Certificate2 LoadCertificate(string thumbprint)
    {
        // path to private ssl certificates in a Linux os Azure App Service
        var bytes = File.ReadAllBytes($"/var/ssl/private/{thumbprint}.p12");
        var certificate = new X509Certificate2(bytes);
        return certificate;
    }

    #endregion

    #region Public Methods

    public static void AddOpenIddictServer(this IServiceCollection services, AppSettings appSettings, IWebHostEnvironment environment)
    {
        services.AddOpenIddict()
                .AddCore(o =>
                         {
                             o.UseEntityFrameworkCore()
                              .UseDbContext<Context>()
                              .ReplaceDefaultEntities<Application, Authorization, Scope, Token, int>();
                         })
                .AddClient(o =>
                           {
                               o.AllowAuthorizationCodeFlow();

                               if (environment.IsDevelopment())
                               {
                                   o.AddDevelopmentEncryptionCertificate()
                                    .AddDevelopmentSigningCertificate();
                               }
                               else
                               {
                                   o.AddEncryptionCertificate(LoadCertificate(appSettings.AuthServerSettings.EncryptionCertificateThumbprint))
                                    .AddSigningCertificate(LoadCertificate(appSettings.AuthServerSettings.SigningCertificateThumbprint));
                               }

                               o.UseAspNetCore()
                                .EnableRedirectionEndpointPassthrough();

                               o.UseSystemNetHttp()
                                .SetProductInformation(typeof(Program).Assembly);

                               o.UseWebProviders()
                                .AddMicrosoft(m =>
                                              {
                                                  var microsoftSettings = appSettings.AuthServerSettings.MicrosoftSettings;
                                                  m.SetClientId(microsoftSettings.ClientId)
                                                   .SetClientSecret(microsoftSettings.ClientSecret)
                                                   .SetRedirectUri(microsoftSettings.RedirectUri)
                                                   .AddScopes("profile"); // must have "profile" to get objectId 
                                              })
                                .AddGoogle(g =>
                                           {
                                               var googleSettings = appSettings.AuthServerSettings.GoogleSettings;
                                               g.SetClientId(googleSettings.ClientId)
                                                .SetClientSecret(googleSettings.ClientSecret)
                                                .SetRedirectUri(googleSettings.RedirectUri)
                                                .AddScopes("openid"); // must have "openid" to get subject 
                                           })
                                .AddReddit(r =>
                                           {
                                               var redditSettings = appSettings.AuthServerSettings.RedditSettings;
                                               r.SetClientId(redditSettings.ClientId)
                                                .SetClientSecret(redditSettings.ClientSecret)
                                                .SetRedirectUri(redditSettings.RedirectUri);
                                           });
                           })
                .AddServer(o =>
                           {
                               o.SetAccessTokenLifetime(TimeSpan.FromMinutes(120))
                                .SetRefreshTokenLifetime(TimeSpan.FromDays(31))
                                .SetIdentityTokenLifetime(TimeSpan.FromMinutes(120));

                               o.AllowAuthorizationCodeFlow()
                                .AllowRefreshTokenFlow();

                               o.SetTokenEndpointUris("/token")
                                .SetAuthorizationEndpointUris("/authorize")
                                .SetIntrospectionEndpointUris("/connect/introspect");

                               if (environment.IsDevelopment())
                               {
                                   o.AddDevelopmentEncryptionCertificate()
                                    .AddDevelopmentSigningCertificate();
                               }
                               else
                               {
                                   o.AddEncryptionCertificate(LoadCertificate(appSettings.AuthServerSettings.EncryptionCertificateThumbprint))
                                    .AddSigningCertificate(LoadCertificate(appSettings.AuthServerSettings.SigningCertificateThumbprint));
                               }

                               o.UseAspNetCore()
                                .EnableTokenEndpointPassthrough()
                                .EnableAuthorizationEndpointPassthrough();
                           })
                .AddValidation(o =>
                               {
                                   o.UseLocalServer();
                                   o.UseAspNetCore();
                               });
    }

    #endregion
}
