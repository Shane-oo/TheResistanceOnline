using Microsoft.AspNetCore.StaticFiles;
using OpenIddict.Validation.AspNetCore;
using TheResistanceOnline.Authentications;
using TheResistanceOnline.Common.ValidationHelpers;
using TheResistanceOnline.Core;
using TheResistanceOnline.Data;
using TheResistanceOnline.Users;
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

        app.UseCustomExceptionHandler();

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
                                                            .WithOrigins(appSettings.ClientUrls);
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

        services.AddDataContext(Configuration);

        services.AddOpenIddictServer(appSettings, Environment);

        services.AddUserIdentity();

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

        // TheResistanceOnline.Data
        services.AddSharedDbQueries();

        // TheResistanceOnline.Authentications
        services.AddAuthenticationDbQueries();
        services.AddAuthenticationServices();

        // TheResistanceOnline.Users
        services.AddUserServices();

        services.AddMediatrBehaviours();
    }

    #endregion
}
