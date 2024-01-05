using Microsoft.AspNetCore.StaticFiles;
using OpenIddict.Validation.AspNetCore;
using TheResistanceOnline.Authentications;
using TheResistanceOnline.Common.ValidationHelpers;
using TheResistanceOnline.Core;
using TheResistanceOnline.Data;
using TheResistanceOnline.Users;
using TheResistanceOnline.Web;
using TheResistanceOnline.Web.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Services

builder.Services.AddApplicationInsightsTelemetry();

var appSettingsSection = builder.Configuration.GetSection(nameof(AppSettings));
var appSettings = appSettingsSection.Get<AppSettings>();
// Validate app settings
ValidateObjectPropertiesHelper.ValidateAllObjectProperties(appSettings,
                                                           appSettings.AuthServerSettings,
                                                           appSettings.AuthServerSettings.MicrosoftSettings,
                                                           appSettings.AuthServerSettings.GoogleSettings,
                                                           appSettings.AuthServerSettings.RedditSettings);
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

builder.Services.AddControllersWithViews();

builder.Services.Configure<StaticFileOptions>(o =>
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

builder.Services.AddDataContext(builder.Configuration);

builder.Services.AddOpenIddictServer(appSettings, builder.Environment);

builder.Services.AddUserIdentity();

builder.Services.AddAuthentication(o =>
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
builder.Services.AddSharedDbQueries();

// TheResistanceOnline.Authentications
builder.Services.AddAuthenticationDbQueries();
builder.Services.AddAuthenticationServices();

// TheResistanceOnline.Users
builder.Services.AddUserServices();

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

app.UseCustomExceptionHandler();

app.MapControllerRoute("default", "{controller}/{action=index}/{id?}");
app.MapFallbackToFile("index.html"); // must have this for angular spa to work


app.Run();
