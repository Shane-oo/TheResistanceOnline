using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheResistanceOnline.BusinessLogic.Core.Queries;
using TheResistanceOnline.BusinessLogic.Users;
using TheResistanceOnline.BusinessLogic.Users.Models;

namespace TheResistanceOnline.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
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

        [HttpGet]
        public async Task<ActionResult<UserDetailsModel>> GetUser()
        {
            try
            {
                var query = new ByIdQuery
                            {
                                UserId = User.FindFirst(ClaimTypes.UserData)?.Value
                            };
                var userDetails = await _userService.GetUserByIdAsync(query);
                return Ok(userDetails);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        // [HttpGet("{userId}")]
        // public async Task<ActionResult<UserDetailsModel>> GetUser([FromRoute] ByIdQuery query)
        // {
        //     try
        //     {
        //         var userDetails = await _userService.GetUserAsync(query);
        //         return Ok(userDetails);
        //     }
        //     catch(Exception ex)
        //     {
        //         return BadRequest(ex.Message);
        //     }
        // }


        // [Route("Roles")]
        // public async Task<ActionResult<List<ModelBase>>> GetRoles([FromRoute] Query query) => await _userService.GetRolesAsync(query);
    }
}
