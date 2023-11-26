using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheResistanceOnline.Users.Users;

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
        SetQuery(query);
        
        var result = await _mediator.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }

    [HttpPost]
    public async Task<ActionResult<UserDetailsModel>> UpdateUser(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        SetCommand(command);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return await GetUser(new GetUserQuery(command.UserId, command.UserRole), cancellationToken);
    }

    #endregion
}
