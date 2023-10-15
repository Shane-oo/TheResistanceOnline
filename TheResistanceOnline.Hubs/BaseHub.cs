using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using OpenIddict.Abstractions;
using TheResistanceOnline.Core.Requests;
using TheResistanceOnline.Data.Entities.UserEntities;

namespace TheResistanceOnline.Hubs;

public class BaseHub<T>: Hub<T> where T : class
{
    #region Properties

    private Guid UserId => Context.User?.Identity is { IsAuthenticated: true }
                           && Guid.TryParse(Context.User.FindFirstValue(OpenIddictConstants.Claims.Subject), out var userId)
                               ? userId
                               : Guid.Empty;

    private Roles UserRole => Context.User?.Identity is { IsAuthenticated: true }
                              && Enum.TryParse<Roles>(Context.User.FindFirstValue(OpenIddictConstants.Claims.Role), out var role)
                                  ? role
                                  : Roles.None;

    #endregion

    #region Private Methods

    protected void SetRequest(IRequestBase request)
    {
        request.UserId = UserId;
        request.UserRole = UserRole;
        request.ConnectionId = Context.ConnectionId;
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
