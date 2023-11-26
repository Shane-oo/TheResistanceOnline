using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using OpenIddict.Abstractions;
using TheResistanceOnline.Core.Errors;
using TheResistanceOnline.Core.Exceptions;
using TheResistanceOnline.Core.NewCommandAndQueriesAndResultsPattern;
using TheResistanceOnline.Data.Entities;
using TheResistanceOnline.Hubs.Common;

namespace TheResistanceOnline.Hubs;

public class BaseHub<THub>: Hub<THub> where THub : class, IErrorHub
{
    #region Fields

    private readonly ILogger<BaseHub<THub>> _logger;

    #endregion

    #region Properties

    private UserId UserId => Context.User?.Identity is { IsAuthenticated: true }
                             && Guid.TryParse(Context.User.FindFirstValue(OpenIddictConstants.Claims.Subject), out var userId)
                                 ? new UserId(userId)
                                 : null;

    private Roles UserRole => Context.User?.Identity is { IsAuthenticated: true }
                              && Enum.TryParse(Context.User.FindFirstValue(OpenIddictConstants.Claims.Role), out Roles userRole)
                                  ? userRole
                                  : Roles.None;

    #endregion

    #region Construction

    public BaseHub(ILogger<BaseHub<THub>> logger)
    {
        _logger = logger;
    }

    public BaseHub()
    {
        
    }

    #endregion

    #region Private Methods

    protected async Task HandleError(Exception ex)
    {
        var exceptionDetails = GetExceptionDetails(ex);
        if (exceptionDetails.Status == StatusCodes.Status500InternalServerError)
        {
            _logger.LogError(ex, "Internal Server Error occured: {Message}", ex.Message);
        }


        if (exceptionDetails.Errors != null && exceptionDetails.Errors.Any())
        {
            await Clients.Caller.Error(exceptionDetails.Errors);
        }
        else
        {
            await Clients.Caller.Error(new Error($"{nameof(THub)}.{exceptionDetails.Title}", exceptionDetails.Detail));
        }
    }

    protected void SetCommand(IBaseCommand command)
    {
        command.UserId = UserId;
        command.UserRole = UserRole;
    }

    protected void SetQuery<T>(Query<T> query)
    {
        query.UserId = UserId;
        query.UserRole = UserRole;
    }

    protected void SetRequest(IConnectionModel request)
    {
        request.ConnectionId = Context.ConnectionId;
    }

    private static ExceptionDetails GetExceptionDetails(Exception exception)
    {
        return exception switch
        {
            ValidationException validationException => new ExceptionDetails(
                                                                            StatusCodes.Status400BadRequest,
                                                                            "ValidationFailure",
                                                                            "Validation error",
                                                                            "One or more validation errors has occured",
                                                                            validationException.Errors),
            _ => new ExceptionDetails(StatusCodes.Status500InternalServerError, "ServerError", "Server Error", "An unexpected error has occured", null)
        };
    }

    #endregion

    private record ExceptionDetails(int Status, string Type, string Title, string Detail, IEnumerable<object> Errors);
}

public class AuthorizeRoles: AuthorizeAttribute
{
    #region Construction

    public AuthorizeRoles(params Roles[] roles)
    {
        var allowedRolesAsStrings = roles.Select(r => r.ToString());
        Roles = string.Join(",", allowedRolesAsStrings);
    }

    #endregion
}
