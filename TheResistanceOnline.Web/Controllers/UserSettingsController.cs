using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheResistanceOnline.BusinessLogic.UserSettings;
using TheResistanceOnline.BusinessLogic.UserSettings.Commands;

namespace TheResistanceOnline.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserSettingsController: ControllerBase
    {
        #region Fields

        private readonly IUserSettingsService _userSettingsService;

        #endregion

        #region Construction

        public UserSettingsController(IUserSettingsService userSettingsService)
        {
            _userSettingsService = userSettingsService;
        }

        #endregion

        #region Public Methods

        [HttpPost]
        [Route("UpdateUserSettings")]
        public async Task<IActionResult> UpdateUserSettings(UserSettingsUpdateCommand command)
        {
            try
            {
                await _userSettingsService.UpdateUserSettingsAsync(command);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion
    }
}
