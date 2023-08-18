using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheResistanceOnline.Core.Exceptions;
using TheResistanceOnline.Users.Users.GetUser;

namespace TheResistanceOnline.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController: ApiControllerBase
{
    #region Fields

    private readonly IMediator _mediator;

    #endregion

    #region Construction

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    #endregion

    #region Public Methods

    //[AuthorizeRoles(Roles.Admin)] 
    [HttpGet]
    public async Task<ActionResult<UserDetailsModel>> GetUser([FromRoute] GetUserQuery query, CancellationToken cancellationToken)
    {
        SetRequest(query);

      
            var userDetails = await _mediator.Send(query, cancellationToken);
            return Ok(userDetails);
       
      
      
      
    }

    #endregion


    // [Route("Roles")]
    // public async Task<ActionResult<List<ModelBase>>> GetRoles([FromRoute] Query query) => await _userService.GetRolesAsync(query);
}
