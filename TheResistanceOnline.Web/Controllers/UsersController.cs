using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheResistanceOnline.Core.Exceptions;
using TheResistanceOnline.Users.Users.GetUser;
using TheResistanceOnline.Users.Users.UpdateUser;

namespace TheResistanceOnline.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController: ApiControllerBase
{
    #region Fields

    private readonly IHostEnvironment _environment;

    private readonly ILogger<UsersController> _logger;
    private readonly IMediator _mediator;

    #endregion

    #region Construction

    public UsersController(IMediator mediator, ILogger<UsersController> logger, IHostEnvironment environment)
    {
        _mediator = mediator;
        _logger = logger;
        _environment = environment;
    }

    #endregion

    #region Public Methods

    //[AuthorizeRoles(Roles.Admin)] 
    [HttpGet]
    public async Task<ActionResult<UserDetailsModel>> GetUser([FromRoute] GetUserQuery query, CancellationToken cancellationToken)
    {
        SetRequest(query);
        try
        {
            var userDetails = await _mediator.Send(query, cancellationToken);
            return Ok(userDetails);
        }
        catch(DomainException ex)
        {
            return BadRequest(ex.Message);
        }
        catch(NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch(UnauthorizedException)
        {
            return Forbid();
        }
        catch(OperationCanceledException)
        {
            return NoContent();
        }
        catch(Exception ex)
        {
            _logger.LogError("{ExMessage}. {InnerExceptionMessage}", ex.Message, ex.InnerException?.Message);
            return Problem(_environment.IsDevelopment() ? ex.Message : "Something Went Wrong");
        }
    }

    [HttpPost]
    public async Task<ActionResult<UserDetailsModel>> UpdateUser(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        SetRequest(command);
        try
        {
            await _mediator.Send(command, cancellationToken);
        }
        catch(DomainException ex)
        {
            return BadRequest(ex.Message);
        }
        catch(NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch(UnauthorizedException)
        {
            return Forbid();
        }
        catch(OperationCanceledException)
        {
            return NoContent();
        }
        catch(Exception ex)
        {
            _logger.LogError("{ExMessage}. {InnerExceptionMessage}", ex.Message, ex.InnerException?.Message);
            return Problem(_environment.IsDevelopment() ? ex.Message : "Something Went Wrong");
        }

        return await GetUser(new GetUserQuery(command.UserId, command.UserRole), cancellationToken);
    }

    #endregion
}
