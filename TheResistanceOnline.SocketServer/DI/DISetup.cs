using System.Text;
using Discord;
using Discord.WebSocket;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TheResistanceOnline.BusinessLogic.DiscordServer;
using TheResistanceOnline.BusinessLogic.Emails;
using TheResistanceOnline.BusinessLogic.Games;
using TheResistanceOnline.BusinessLogic.Timers;
using TheResistanceOnline.BusinessLogic.Users;
using TheResistanceOnline.BusinessLogic.Users.DbQueries;
using TheResistanceOnline.BusinessLogic.UserSettings;
using TheResistanceOnline.Data;
using TheResistanceOnline.Data.Users;
using TheResistanceOnline.Infrastructure.Data;
using TheResistanceOnline.Infrastructure.Data.Interceptors.CoreInterceptors;
using TheResistanceOnline.Infrastructure.Data.Queries.Users;

namespace TheResistanceOnline.SocketServer.DI
{
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

            services.AddAuthentication(x =>
                                       {
                                           x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                                           x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                                       })
                    .AddJwtBearer(x =>
                                  {
                                      x.RequireHttpsMetadata = true;
                                      x.SaveToken = true;
                                      x.TokenValidationParameters = new TokenValidationParameters
                                                                    {
                                                                        ValidateIssuerSigningKey = true,
                                                                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                                                                                                                        .GetBytes(_securityKey)),
                                                                        ValidateIssuer = false,
                                                                        ValidateAudience = false
                                                                    };
                                      x.Events = new JwtBearerEvents
                                                 {
                                                     // So on every message the server recieves get the token from the this.options
                                                     // which has the token
                                                     OnMessageReceived = context =>
                                                                         {
                                                                             var accessToken = context.Request.Query["access_token"];

                                                                             // If the request is for our hub...
                                                                             var path = context.HttpContext.Request.Path;
                                                                             // shane if you running into problems with the SignalR auth its cause of this stupid path segment
                                                                             if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/theresistancehub"))
                                                                             {
                                                                                 // Read the token out of the query string
                                                                                 context.Token = accessToken;
                                                                             }

                                                                             return Task.CompletedTask;
                                                                         }
                                                 };
                                  });
        }

        public static void AddContext(this IServiceCollection services)
        {
            if (_connectionString == null)
            {
                throw new NullReferenceException("Connection string not found");
            }

            services.AddDbContext<Context>((sp, options) =>
                                           {
                                               var auditableInterceptor = sp.GetService<UpdateAuditableEntitiesInterceptor>();
                                               options.UseSqlServer(_connectionString)
                                                      .AddInterceptors(auditableInterceptor ?? throw new InvalidOperationException("auditableInterceptor cannot be null"));
                                           });
            services.AddScoped<IDataContext, DataContext>();
        }

        public static void AddServices(this IServiceCollection services)
        {
            // Services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserIdentityManager, UserIdentityManager>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IDiscordServerService, DiscordServerService>();
            services.AddScoped<IUserSettingsService, UserSettingsService>();
            services.AddScoped<IGameService, GameService>();
          
            //ToDo probs not needed
            services.AddScoped<ITimerService, TimerService>();

            // Queries
            services.AddTransient<IUserByNameOrEmailDbQuery, UserByNameOrEmailDbQuery>();
            services.AddTransient<IUserDbQuery, UserDbQuery>();

            // Interceptors
            services.AddSingleton<UpdateAuditableEntitiesInterceptor>();

            // Mapping Profiles
            services.AddAutoMapper(typeof(UserMappingProfile));
            services.AddAutoMapper(typeof(DiscordServerMappingProfile));

            // Identities
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
            // Discord Services

            services.AddSingleton(new DiscordSocketConfig
                                  {
                                      AlwaysDownloadUsers = true,
                                      GatewayIntents = GatewayIntents.All
                                  });
            services.AddSingleton<DiscordSocketClient>();
        }

        #endregion
    }
}
