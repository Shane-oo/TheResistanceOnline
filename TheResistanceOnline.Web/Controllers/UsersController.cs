using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheResistanceOnline.BusinessLogic.Users;
using TheResistanceOnline.BusinessLogic.Users.Commands;
using TheResistanceOnline.Data.Exceptions;

namespace TheResistanceOnline.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UsersController: ControllerBase
    {
        #region Fields

        private readonly IUserService _userService;

        #endregion

        #region Construction

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        #endregion

        #region Public Methods

        [HttpPost]
        [Route("GetUser")]
        [Authorize]
        public async Task<IActionResult> GetUserAsync(GetUserCommand command)
        {
            Console.WriteLine();
            try
            {
                var userDetails = await _userService.GetUserAsync(command);
                //return Ok(userLoginResponse);
                return Ok(userDetails);
            }
            catch(DomainException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        #endregion
    }
}
