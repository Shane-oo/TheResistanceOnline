using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;
using OpenIddict.Validation.AspNetCore;
using TheResistanceOnline.Common.ValidationHelpers;
using TheResistanceOnline.Core;
using TheResistanceOnline.Data;
using TheResistanceOnline.Data.Entities.UserEntities;
using TheResistanceOnline.Data.Interceptors;
using TheResistanceOnline.Data.Queries.UserQueries;
using TheResistanceOnline.Games.Lobbies;
using TheResistanceOnline.Games.Lobbies.CreateLobby;
using TheResistanceOnline.Games.Streams;

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
                             endpoints.MapHub<StreamHub>("/stream");
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
                                                            .WithOrigins(appSettings.ClientUrl);
                                                       });
                         });

        services.AddSignalR();


        services.AddSingleton<UpdateAuditableEntitiesInterceptor>();
        services.AddDbContext<Context>((sp, o) =>
                                       {
                                           var connectionString = Configuration.GetConnectionString("ResistanceDb");
                                           ArgumentException.ThrowIfNullOrEmpty(connectionString);
                                           var auditableInterceptor = sp.GetService<UpdateAuditableEntitiesInterceptor>();

                                           o.UseSqlServer(connectionString)
                                            .AddInterceptors(auditableInterceptor);
                                       });
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
                                   });

        // Data Context
        services.AddScoped<IDataContext, DataContext>();

        // Db Queries
        // TheResistanceOnline.Data
        services.AddTransient<IUserByUserIdDbQuery, UserByUserIdDbQuery>();

        var assemblies = new[]
                         {
                             // TheResistanceOnline.Games
                             typeof(CreateLobbyHandler).Assembly
                         };
        // MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies));

        // AutoMapper
        //services.AddAutoMapper(assemblies);

        // FluentValidation
        foreach(var assembly in assemblies)
        {
            services.AddValidatorsFromAssembly(assembly);
        }

        // Services
        services.AddSingleton<LobbyHubPersistedProperties>();
        services.AddSingleton<StreamHubPersistedProperties>();
    }

    #endregion
}
