using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using TheResistanceOnline.Core.NewCommandAndQueriesAndResultsPattern;
using TheResistanceOnline.Data.Entities;

namespace TheResistanceOnline.Web.Controllers;

[ApiController]
public class ApiControllerBase: ControllerBase
{
    #region Properties

    private UserId UserId => User.Identity is { IsAuthenticated: true }
                             && Guid.TryParse(User.FindFirstValue(OpenIddictConstants.Claims.Subject), out var userIdGuid)
                                 ? new UserId(userIdGuid)
                                 : null;

    private Roles UserRole => User.Identity is { IsAuthenticated: true }
                              && Enum.TryParse(User.FindFirstValue(OpenIddictConstants.Claims.Role), out Roles userRole)
                                  ? userRole
                                  : Roles.None;

    #endregion

    #region Private Methods

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

    #endregion
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
