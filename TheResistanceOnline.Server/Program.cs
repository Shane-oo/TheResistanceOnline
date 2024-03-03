using OpenIddict.Validation.AspNetCore;
using TheResistanceOnline.Common.ValidationHelpers;
using TheResistanceOnline.Core;
using TheResistanceOnline.Data;
using TheResistanceOnline.Server;
using TheResistanceOnline.Server.Lobbies;
using TheResistanceOnline.Server.Resistance;
using TheResistanceOnline.Users;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationInsightsTelemetry();

var appSettingsSection = builder.Configuration.GetSection(nameof(AppSettings));
var appSettings = appSettingsSection.Get<AppSettings>();
// Validate app settings
ValidateObjectPropertiesHelper.ValidateAllObjectProperties(appSettings, appSettings.SocketServerSettings);
builder.Services.Configure<AppSettings>(appSettingsSection);

builder.Services.AddCors(o =>
                         {
                             o.AddPolicy("CorsPolicy", p =>
                                                       {
                                                           p.AllowAnyMethod()
                                                            .AllowAnyHeader()
                                                            .AllowCredentials()
                                                            .WithOrigins(appSettings.ClientUrls);
                                                       });
                         });

builder.Services.AddSignalR();

builder.Services.AddDataContext(builder.Configuration);

builder.Services.AddOpenIddictIntrospection(appSettings, builder.Environment);

builder.Services.AddUserIdentity();

builder.Services.AddAuthentication(o =>
                                   {
                                       o.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
                                       o.DefaultAuthenticateScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
                                       o.DefaultChallengeScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
                                   });

// Data Context
builder.Services.AddScoped<IDataContext, DataContext>();

// Db Queries
// TheResistanceOnline.Data
builder.Services.AddSharedDbQueries();

// TheResistanceOnline.Hubs
builder.Services.AddHubServices();

builder.Services.AddMediatrBehaviours();

// App
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<LobbyHub>("/lobby");
//app.MapHub<StreamHub>("/stream"); // stream not supported right now -> will revisit this later
app.MapHub<ResistanceHub>("/resistance");

app.Run();
