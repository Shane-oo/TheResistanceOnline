using Microsoft.AspNetCore.Mvc;
using TheResistanceOnline.BusinessLogic.Games.Commands;

namespace TheResistanceOnline.SocketServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController: ControllerBase
    {
        #region Public Methods

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateGame(CreateGameCommand command)
        {
            try
            {
                Console.WriteLine(command);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        #endregion
    }
}
