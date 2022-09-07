using TheResistanceOnline.BusinessLogic.Games;
using TheResistanceOnline.SocketServer.DI;
using TheResistanceOnline.SocketServer.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
                         {
                             options.AddPolicy("CorsPolicy", corsPolicy => corsPolicy
                                                                           .AllowAnyMethod()
                                                                           .AllowAnyHeader()
                                                                           .AllowCredentials()
                                                                           .WithOrigins("http://localhost:44452", "https://theresistanceboardgameonline.com",
                                                                                        "https://localhost:44452")
                                              );
                         });

builder.Services.AddServices();
builder.Services.AddAuthenticationServices();
builder.Services.AddContext();

builder.Services.AddSignalR();

builder.Services.AddControllers();


var app = builder.Build();


app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
                 {
                     endpoints.MapControllers();
                     //todo map hubs dont forget
                     //endpoints.MapHub<TheResistanceHub>("/message");
                     endpoints.MapHub<TheResistanceHub>("/theresistancehub");        //// path will look like this https://localhost:44379/theresistancehub 
                 });

app.Run();


//  app.UseSignalR(route =>
// {
//     route.MapHub<ChatHub>("/chathub");
// });