using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace TheResistanceOnline.Server.Hubs;

[Authorize]
public class LobbyHub: BaseHub
{
    public override async Task OnConnectedAsync()
    {

        Console.WriteLine(UserId);
        
        await base.OnConnectedAsync();
    }
}
