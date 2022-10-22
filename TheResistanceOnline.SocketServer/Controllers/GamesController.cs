using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheResistanceOnline.BusinessLogic.Games;

namespace TheResistanceOnline.SocketServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GamesController: ControllerBase
    {
        #region Fields


        #endregion

        #region Construction

        public GamesController()
        {
          
        }

        #endregion
    }
}
