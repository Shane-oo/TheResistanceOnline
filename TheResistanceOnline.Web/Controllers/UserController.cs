using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheResistanceOnline.BusinessLogic.Users;
using TheResistanceOnline.BusinessLogic.Users.Commands;

namespace TheResistanceOnline.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController: ControllerBase
    {
        #region Fields

        private readonly IUserService _userService;

        #endregion

        #region Construction

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        #endregion

        #region Public Methods

        [HttpPost]
        [Authorize]
        [Route("GetUser")]
        public async Task<IActionResult> GetUser(GetUserCommand command)
        {
            try
            {
                var userDetails = await _userService.GetUserAsync(command);
                return Ok(userDetails);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion
    }
}
