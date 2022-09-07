using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using TheResistanceOnline.BusinessLogic.Games;
using TheResistanceOnline.BusinessLogic.Games.Commands;

namespace TheResistanceOnline.SocketServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GamesController: ControllerBase
    {
        #region Fields

        private readonly IGameService _gameService;

        #endregion

        #region Construction

        public GamesController( IGameService gameService)
        {
            _gameService = gameService;
  
        }

        #endregion

        #region Public Methods
        

        #endregion
    }
}
