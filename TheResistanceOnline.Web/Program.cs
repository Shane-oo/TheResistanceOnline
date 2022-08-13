using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using TheResistanceOnline.Data.Users;
using TheResistanceOnline.Infrastructure.Data;
using TheResistanceOnline.Web.DI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
// Add services to the container.
builder.Services.AddCors(options =>
                         {
                             options.AddPolicy("CorsPolicy", corsPolicy => corsPolicy
                                                                           .AllowAnyMethod()
                                                                           .AllowAnyHeader()
                                                                           .AllowCredentials()
                                                                           .WithOrigins("https://theresistanceboardgameonline.com",
                                                                                        "https://localhost:44452")
                                              );
                         });
builder.Services.Configure<StaticFileOptions>(options =>
                                              {
                                                  options.ContentTypeProvider = new FileExtensionContentTypeProvider
                                                                                {
                                                                                    Mappings =
                                                                                    {
                                                                                        [".gltf"] = "model/gltf+json",
                                                                                        [".glb"] = "model/gltf-binary",
                                                                                        [".bin"] = "application/octet-stream"
                                                                                    }
                                                                                };
                                              });
builder.Services.AddServices();
// TODO: kubernetes secret get ready
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.AddAuthenticationServices(jwtSettings);
// TODO: kubernetes secret get ready
var connectionString = builder.Configuration.GetConnectionString("resistanceDb");
builder.Services.AddContext(connectionString);




var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
                       "default",
                       "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");


app.Run();
