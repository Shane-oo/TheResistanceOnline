using Microsoft.AspNetCore.SignalR;
using TheResistanceOnline.BusinessLogic.Games.Commands;
using TheResistanceOnline.BusinessLogic.Games.Models;

namespace TheResistanceOnline.BusinessLogic.Games
{
    public interface IGameService
    {
    }

    public class GameService: IGameService
    {
        #region Fields

        private readonly IHubContext<TheResistanceHub, ITheResistanceHub> _hubContext;

        #endregion

        #region Construction

        public GameService(IHubContext<TheResistanceHub, ITheResistanceHub> hubContext)
        {
            _hubContext = hubContext;
        }

        #endregion

        #region Public Methods
        
        public async Task sendMessage()
        {
            var messageModel = new MessageModel
                               {
                                   User = "Server",
                                   Message = "Hello from server"
                               };
            await _hubContext.Clients.All.ReceivedMessage(messageModel);
        }

        #endregion
    }
}
