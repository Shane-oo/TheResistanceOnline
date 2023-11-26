using OpenIddict.Validation.AspNetCore;
using TheResistanceOnline.Common.ValidationHelpers;
using TheResistanceOnline.Core;
using TheResistanceOnline.Data;
using TheResistanceOnline.Hubs;
using TheResistanceOnline.Hubs.Lobbies;
using TheResistanceOnline.Hubs.Resistance;
using TheResistanceOnline.Hubs.Streams;
using TheResistanceOnline.Users;

namespace TheResistanceOnline.Server;

public class Startup
{
    #region Properties

    private IConfiguration Configuration { get; }

    private IWebHostEnvironment Environment { get; }

    #endregion

    #region Construction

    public Startup(IConfiguration configuration, IWebHostEnvironment environment)
    {
        Configuration = configuration;
        Environment = environment;
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
                             endpoints.MapHub<LobbyHub>("/lobby");
                             //endpoints.MapHub<StreamHub>("/stream"); // stream not supported right now -> will revisit this later
                             endpoints.MapHub<ResistanceHub>("/resistance");
                         });
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddApplicationInsightsTelemetry();

        var appSettingsSection = Configuration.GetSection(nameof(AppSettings));
        var appSettings = appSettingsSection.Get<AppSettings>();
        // Validate app settings
        ValidateObjectPropertiesHelper.ValidateAllObjectProperties(appSettings, appSettings.SocketServerSettings);
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

        services.AddSignalR();

        services.AddDataContext(Configuration);

        services.AddOpenIddictIntrospection(appSettings, Environment);

        services.AddUserIdentity();

        services.AddAuthentication(o =>
                                   {
                                       o.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
                                       o.DefaultAuthenticateScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
                                       o.DefaultChallengeScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
                                   });

        // Data Context
        services.AddScoped<IDataContext, DataContext>();

        // Db Queries
        // TheResistanceOnline.Data
        services.AddSharedDbQueries();

        // TheResistanceOnline.Hubs
        services.AddHubServices();

        services.AddMediatrBehaviours();
    }

    #endregion
}
