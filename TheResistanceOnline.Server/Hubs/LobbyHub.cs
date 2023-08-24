using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using TheResistanceOnline.Data.Entities.UserEntities;

namespace TheResistanceOnline.Server.Hubs;

[AuthorizeRoles(Roles.Admin)] 
public class LobbyHub: BaseHub
{
    public override async Task OnConnectedAsync()
    {

        Console.WriteLine(UserId);
        
        await base.OnConnectedAsync();
    }
}
