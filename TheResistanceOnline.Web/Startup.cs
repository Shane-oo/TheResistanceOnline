using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Validation.AspNetCore;
using TheResistanceOnline.Common.ValidationHelpers;
using TheResistanceOnline.Data;
using TheResistanceOnline.Data.Entities.AuthorizationEntities;
using TheResistanceOnline.Data.Interceptors;
using TheResistanceOnline.Data.Queries.UserQueries;

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
        ValidateObjectPropertiesHelper.ValidateAllObjectProperties(appSettings, appSettings.AuthServer);
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
                //todo .AddClient()
                .AddServer(o =>
                           {
                               o.SetAccessTokenLifetime(TimeSpan.FromHours(1))
                                .SetRefreshTokenLifetime(TimeSpan.FromDays(14))
                                .SetIdentityTokenLifetime(TimeSpan.FromHours(1));

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
                                   o.AddEncryptionCertificate(appSettings.AuthServer.EncryptionCertificateThumbprint)
                                    .AddSigningCertificate(appSettings.AuthServer.SigningCertificateThumbprint);
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

        //todo AddIdentity

        services.AddAuthentication(o =>
                                   {
                                       o.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
                                       o.DefaultAuthenticateScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
                                       o.DefaultChallengeScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
                                   })
                .AddCookie(o =>
                           {
                               o.Cookie.Name = "TODO";
                               o.SlidingExpiration = false;
                           });

        // Dependency Injection
        // Data Context
        services.AddScoped<IDataContext, DataContext>();

        // Db Queries
        services.AddTransient<IUserByNameOrEmailDbQuery, UserByNameOrEmailDbQuery>();
        services.AddTransient<IUserDbQuery, UserDbQuery>();

        // MediatR
        // TheResistanceOnline.Authentications
        // todo
    }
}
