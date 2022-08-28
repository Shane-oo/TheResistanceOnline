using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using TheResistanceOnline.BusinessLogic.Games.Models;

namespace TheResistanceOnline.BusinessLogic.Games
{
    public interface ITheResistanceHub
    {
        // Define events functions
        Task ReceivedMessage(MessageModel message);

        Task SendAsync(string receivesystemmessage, string s);
    }

    public class TheResistanceHub: Hub<ITheResistanceHub>
    {
        #region Fields
        // only need this if wanting to do some funky logic

        //private static readonly ConcurrentDictionary<string, string> _connections = new ConcurrentDictionary<string, string>();

        #endregion

        #region Public Methods
        
        // only need this if wanting to do some funky logic
        // public override async Task OnConnectedAsync()
        // {
        //     var user = Context.User?.Identity?.Name;
        //     if (user != null)
        //     {
        //         _connections.AddOrUpdate(Context.ConnectionId, user, (key, oldValue) => user);
        //     }
        //     var httpContext = Context.GetHttpContext();
        //
        //     if (httpContext != null)
        //     {
        //         var jwtToken = httpContext.Request.Query["access_token"];
        //         var handler = new JwtSecurityTokenHandler();
        //         if (!string.IsNullOrEmpty(jwtToken))
        //         {
        //             var token = handler.ReadJwtToken(jwtToken);
        //             var tokenS = token as JwtSecurityToken;
        //
        //             // replace email with your claim name
        //             //http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name
        //             var jti = tokenS.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;
        //             if (jti != null && jti != "")
        //             {
        //                // Groups.AddToGroupAsync(Context.ConnectionId, jti);
        //             }
        //         }
        //     }
        //
        //
        //     await Clients.All.SendAsync("ReceiveSystemMessage",
        //                                 $"{Context.UserIdentifier} joined.");
        //     await base.OnConnectedAsync();
        // }


        public async Task SendMessage(string user, string message)
        {
            var messageModel = new MessageModel
                               {
                                   User = user,
                                   Message = message
                               };
            await Clients.All.ReceivedMessage(messageModel);
        }
        [Authorize]
        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }
        [Authorize]
        public async Task BroadcastMessageData(string message)
        {
            await Clients.All.SendAsync("broadcastmessagedata", message);
        }
        #endregion
    }
}
