using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheResistanceOnline.BusinessLogic.Users;
using TheResistanceOnline.BusinessLogic.Users.Commands;
using TheResistanceOnline.BusinessLogic.Users.Models;
using TheResistanceOnline.Data.Exceptions;

namespace TheResistanceOnline.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController: ControllerBase
    {
        #region Fields

        private readonly IUserService _userService;

        #endregion

        #region Construction

        public AccountsController(IUserService userService)
        {
            _userService = userService;
        }

        #endregion

        #region Public Methods

        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> RegisterUser([NotNull] UserRegisterCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                await _userService.CreateUserAsync(command);
            }
            catch(DomainException ex)
            {
                return BadRequest(ex.Message);
            }

            // if CreateUserAsync() doesn't throw => successfully registered
            return Ok();
        }

        #endregion
    }
}
