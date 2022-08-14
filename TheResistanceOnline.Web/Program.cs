using Microsoft.AspNetCore.StaticFiles;
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
builder.Services.AddAuthenticationServices();
builder.Services.AddContext();


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


app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

app.Run();
