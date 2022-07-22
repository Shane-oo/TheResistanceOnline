using Microsoft.AspNetCore.SignalR;

namespace TheResistanceOnline.SocketServer.HubConfigurations
{
    public class TheResistanceHub: Hub
    {
        #region Public Methods

        public async Task BroadcastMessageData(string message)
        {
            await Clients.All.SendAsync("broadcastmessagedata", message);
        }

        #endregion
    }
}
