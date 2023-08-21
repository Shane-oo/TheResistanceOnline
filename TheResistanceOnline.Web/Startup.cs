using System.Security.Cryptography.X509Certificates;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;
using OpenIddict.Validation.AspNetCore;
using TheResistanceOnline.Authentications.ExternalIdentities.AuthenticateUserWithGoogle;
using TheResistanceOnline.Authentications.ExternalIdentities.AuthenticateUserWithMicrosoft;
using TheResistanceOnline.Common.ValidationHelpers;
using TheResistanceOnline.Core;
using TheResistanceOnline.Data;
using TheResistanceOnline.Data.Entities.AuthorizationEntities;
using TheResistanceOnline.Data.Entities.UserEntities;
using TheResistanceOnline.Data.Interceptors;
using TheResistanceOnline.Data.Queries.UserQueries;
using TheResistanceOnline.Users.Users.GetUser;
using TheResistanceOnline.Web.Controllers;

namespace TheResistanceOnline.Web;

public class Startup
{
    #region Properties

    private IConfiguration Configuration { get; }

    private IWebHostEnvironment Environment { get; }

    #endregion

    #region Construction

    public Startup(IConfiguration configuration, IWebHostEnvironment env)
    {
        Configuration = configuration;
        Environment = env;
    }

    #endregion

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

    public void Configure(IApplicationBuilder app, IWebHostEnvironment environment)
    {
        // Configure the HTTP request pipeline.
        if (!environment.IsDevelopment())
        {
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseCors("CorsPolicy");

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
                         {
                             endpoints.MapControllerRoute("default", "{controller}/{action=index}/{id?}");
                             endpoints.MapFallbackToFile("index.html"); // must have this for angular spa to work
                         });
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddApplicationInsightsTelemetry();

        var appSettingsSection = Configuration.GetSection(nameof(AppSettings));
        var appSettings = appSettingsSection.Get<AppSettings>();
        // Validate app settings
        ValidateObjectPropertiesHelper.ValidateAllObjectProperties(appSettings,
                                                                   appSettings.AuthServerSettings,
                                                                   appSettings.AuthServerSettings.MicrosoftSettings);
        services.Configure<AppSettings>(appSettingsSection);

        services.AddCors(o =>
                         {
                             o.AddPolicy("CorsPolicy", p =>
                                                       {
                                                           p.AllowAnyMethod()
                                                            .AllowAnyHeader()
                                                            .AllowCredentials()
                                                            .WithOrigins(appSettings.ClientUrl);
                                                       });
                         });

        services.AddControllersWithViews();

        services.Configure<StaticFileOptions>(o =>
                                              {
                                                  o.ContentTypeProvider = new FileExtensionContentTypeProvider
                                                                          {
                                                                              Mappings =
                                                                              {
                                                                                  [".gltf"] = "model/gltf+json",
                                                                                  [".glb"] = "model/gltf-binary",
                                                                                  [".bin"] = "application/octet-stream"
                                                                              }
                                                                          };
                                                  o.ServeUnknownFileTypes = true;
                                              });

        services.AddSingleton<UpdateAuditableEntitiesInterceptor>();
        services.AddDbContext<Context>((sp, o) =>
                                       {
                                           var connectionString = Configuration.GetConnectionString("ResistanceDb");
                                           ArgumentException.ThrowIfNullOrEmpty(connectionString);
                                           var auditableInterceptor = sp.GetService<UpdateAuditableEntitiesInterceptor>();

                                           o.UseSqlServer(connectionString)
                                            .AddInterceptors(auditableInterceptor
                                                             ?? throw new InvalidOperationException("auditableInterceptor cannot be null"));

                                           o.UseOpenIddict<Application, Authorization, Scope, Token, int>();
                                       });

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

                               if (Environment.IsDevelopment())
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
                                           });
                           })
                .AddServer(o =>
                           {
                               o.SetAccessTokenLifetime(TimeSpan.FromMinutes(90))
                                .SetRefreshTokenLifetime(TimeSpan.FromDays(31))
                                .SetIdentityTokenLifetime(TimeSpan.FromMinutes(90));

                               o.AllowAuthorizationCodeFlow()
                                .AllowRefreshTokenFlow();

                               o.SetTokenEndpointUris("/token")
                                .SetAuthorizationEndpointUris("/authorize");

                               if (Environment.IsDevelopment())
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

        services.AddIdentity<User, Role>(o =>
                                         {
                                             o.ClaimsIdentity.UserNameClaimType = OpenIddictConstants.Claims.Name;
                                             o.ClaimsIdentity.UserIdClaimType = OpenIddictConstants.Claims.Subject;
                                             o.ClaimsIdentity.RoleClaimType = OpenIddictConstants.Claims.Role;
                                             o.User.RequireUniqueEmail = false; // disables user needs email to create
                                         })
                .AddEntityFrameworkStores<Context>()
                .AddDefaultTokenProviders();

        services.AddAuthentication(o =>
                                   {
                                       o.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
                                       o.DefaultAuthenticateScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
                                       o.DefaultChallengeScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
                                   })
                .AddCookie(o =>
                           {
                               o.Cookie.Name = AuthorizationsController.COOKIE_NAME;
                               o.SlidingExpiration = false;
                           });

        // Dependency Injection
        // Data Context
        services.AddScoped<IDataContext, DataContext>();
        // Services

        // Db Queries
        // TheResistanceOnline.Data
        services.AddTransient<IUserByNameOrEmailDbQuery, UserByNameOrEmailDbQuery>();
        services.AddTransient<IUserByUserIdDbQuery, UserByUserIdDbQuery>();
        // TheResistanceOnline.Authentications
        services.AddTransient<IMicrosoftUserByObjectIdDbQuery, MicrosoftUserByObjectIdDbQuery>();
        services.AddTransient<IGoogleUserBySubjectDbQuery, GoogleUserBySubjectDbQuery>();

        var assemblies = new[]
                         {
                             // TheResistanceOnline.Authentications
                             typeof(AuthenticateUserWithMicrosoftHandler).Assembly,
                             // TheResistanceOnline.Users
                             typeof(GetUserHandler).Assembly
                         };
        // MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies));

        // AutoMapper
        services.AddAutoMapper(assemblies);

        // FluentValidation
        foreach(var assembly in assemblies)
        {
            services.AddValidatorsFromAssembly(assembly);
        }
    }

    #endregion
}
