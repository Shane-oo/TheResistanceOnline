using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheResistanceOnline.BusinessLogic.DiscordServer;
using TheResistanceOnline.BusinessLogic.DiscordServer.Commands;

namespace TheResistanceOnline.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DiscordServersController: ControllerBase
    {
        #region Fields

        private readonly IDiscordServerService _discordServerService;

        #endregion

        #region Construction

        public DiscordServersController(IDiscordServerService discordServerService)
        {
            _discordServerService = discordServerService;
        }

        #endregion

        #region Public Methods

        [HttpPost]
        [Route("CreateDiscordUser")]
        public async Task<IActionResult> CreateDiscordUser(CreateDiscordUserCommand command)

        {
            try
            {
                command.UserId = User.FindFirst(ClaimTypes.UserData)?.Value;
                await _discordServerService.CreateDiscordUserAsync(command);
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
