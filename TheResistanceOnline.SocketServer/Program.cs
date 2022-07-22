using TheResistanceOnline.BusinessLogic.Timers;
using TheResistanceOnline.SocketServer.DI;
using TheResistanceOnline.SocketServer.HubConfigurations;

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

builder.Services.AddSignalR();

builder.Services.AddControllers();

// DISetup
//builder.Services.AddServices();
builder.Services.AddTransient<ITimerService, TimerService>();

var app = builder.Build();


app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("CorsPolicy");

app.UseAuthorization();

app.UseEndpoints(endpoints =>
                 {
                     endpoints.MapControllers();
                     endpoints.MapHub<TheResistanceHub>("/message");
                 });

app.Run();
