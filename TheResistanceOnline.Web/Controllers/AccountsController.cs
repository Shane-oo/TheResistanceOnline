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
        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(UserForgotPasswordCommand command)
        {
            try
            {
                await _userService.SendResetPasswordAsync(command);
            }
            catch(DomainException ex)
            {
                return BadRequest(ex.Message);
            }
            // if SendResetPasswordAsync() doesn't throw => successfully sent reset email

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(UserLoginCommand command)
        {
            var token = "";
            try
            {
                token = await _userService.LoginUserAsync(command);
            }
            catch(DomainException ex)
            {
                return Unauthorized(ex.Message);
            }

            return Ok(new UserLoginResponse
                      {
                          Token = token
                      });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> RegisterUser(UserRegisterCommand command)
        {
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

        [HttpPost]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword(UserResetPasswordCommand command)
        {
            try
            {
                await _userService.ResetUserPasswordAsync(command);
            }
            catch(DomainException ex)
            {
                return BadRequest(ex.Message);
            }

            // if ResetUserPasswordAsync() doesn't throw => successfully reset password

            return Ok();
        }

        #endregion
    }
}
