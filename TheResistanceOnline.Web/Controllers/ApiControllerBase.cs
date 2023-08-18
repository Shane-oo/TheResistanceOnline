using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using TheResistanceOnline.Core;
using TheResistanceOnline.Core.Requests;
using TheResistanceOnline.Data.Entities.UserEntities;

namespace TheResistanceOnline.Web.Controllers;

[ApiController]
public class ApiControllerBase: ControllerBase
{
    #region Properties

    private Guid UserId => User.Identity is { IsAuthenticated: true }
                           && Guid.TryParse(User.FindFirstValue(OpenIddictConstants.Claims.Subject), out var userId)
                               ? userId
                               : Guid.Empty;

    private Roles UserRole => User.Identity is { IsAuthenticated: true }
                              && Enum.TryParse<Roles>(User.FindFirstValue(OpenIddictConstants.Claims.Role), out var role)
                                  ? role
                                  : Roles.None;

    #endregion

    protected void SetRequest(IRequestBase request)
    {
        request.UserId = UserId;
        request.UserRole = UserRole;
    }
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
