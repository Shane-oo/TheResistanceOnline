// using Microsoft.AspNetCore.StaticFiles;
// using TheResistanceOnline.Web.DI;
//
// var builder = WebApplication.CreateBuilder(args);
//
// // Add services to the container.
//
// builder.Services.AddAppSettings(builder);
// builder.Services.AddControllersWithViews();
// // Add services to the container.
// builder.Services.AddCors(options =>
//                          {
//                              options.AddPolicy("CorsPolicy", corsPolicy => corsPolicy
//                                                                            .AllowAnyMethod()
//                                                                            .AllowAnyHeader()
//                                                                            .AllowCredentials()
//                                                                            .WithOrigins("https://theresistanceboardgameonline.com",
//                                                                                         "https://localhost:44452")
//                                               );
//                          });
// builder.Services.Configure<StaticFileOptions>(options =>
//                                               {
//                                                   options.ContentTypeProvider = new FileExtensionContentTypeProvider
//                                                                                 {
//                                                                                     Mappings =
//                                                                                     {
//                                                                                         [".gltf"] = "model/gltf+json",
//                                                                                         [".glb"] = "model/gltf-binary",
//                                                                                         [".bin"] = "application/octet-stream"
//                                                                                     }
//                                                                                 };
//                                                   options.ServeUnknownFileTypes = true; // enable serving of unknown file types
//                                               });
// builder.Services.AddServices();
// builder.Services.AddAuthenticationServices();
// builder.Services.AddContext();
// builder.Services.AddMemoryCache();
//
//
// var app = builder.Build();
//
//
// // Configure the HTTP request pipeline.
// if (!app.Environment.IsDevelopment())
// {
//     // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//     app.UseHsts();
// }
//
// app.UseHttpsRedirection();
// app.UseStaticFiles();
// app.UseRouting();
// app.UseCors("CorsPolicy");
// app.UseAuthentication();
//
// app.UseAuthorization();
//
// // must for angular spa
// app.MapControllerRoute(
//                        "default",
//                        "{controller}/{action=Index}/{id?}");
// // must for angular spa
// app.MapFallbackToFile("index.html");
//
// app.Run();


using Microsoft.AspNetCore;

namespace TheResistanceOnline.Web;

public class Program
{
    #region Private Methods

    private static IWebHostBuilder CreateWebHostBuilder(string[] args)
    {
        return WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();
    }

    #endregion

    #region Public Methods

    public static void Main(string[] args)
    {
        CreateWebHostBuilder(args).Build().Run();
    }

    #endregion
}