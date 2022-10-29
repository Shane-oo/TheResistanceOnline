using Microsoft.AspNetCore.Mvc;
using TheResistanceOnline.BusinessLogic.Users;
using TheResistanceOnline.BusinessLogic.Users.Commands;

namespace TheResistanceOnline.Web.Controllers;

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

    [HttpPost]
    [Route("ConfirmEmail")]
    public async Task<IActionResult> ConfirmEmail(UserConfirmEmailCommand command)
    {
        try
        {
            await _userService.ConfirmUserEmailAsync(command);
            return Ok();
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [Route("ForgotPassword")]
    public async Task<IActionResult> ForgotPassword(UserForgotPasswordCommand command)
    {
        try
        {
            await _userService.SendResetPasswordAsync(command);
            return Ok();
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> Login(UserLoginCommand command)
    {
        try
        {
            var userLoginResponse = await _userService.LoginUserAsync(command);
            return Ok(userLoginResponse);
        }
        catch(Exception ex)
        {
            return Unauthorized(ex.Message);
        }
    }

    [HttpPost]
    [Route("Register")]
    public async Task<IActionResult> RegisterUser(UserRegisterCommand command)
    {
        try
        {
            await _userService.CreateUserAsync(command);
            return Ok();
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [Route("ResetPassword")]
    public async Task<IActionResult> ResetPassword(UserResetPasswordCommand command)
    {
        try
        {
            await _userService.ResetUserPasswordAsync(command);
            return Ok();
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    #endregion
}
