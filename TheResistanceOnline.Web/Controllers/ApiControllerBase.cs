using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using TheResistanceOnline.Core.Requests;
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

    #endregion

    #region Private Methods

    protected void SetRequest(IRequestBase request)
    {
        request.UserId = UserId;
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
