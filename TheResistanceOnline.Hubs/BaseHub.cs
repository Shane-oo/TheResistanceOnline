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

    protected Guid UserId => Context.User?.Identity is { IsAuthenticated: true }
                             && Guid.TryParse(Context.User.FindFirstValue(OpenIddictConstants.Claims.Subject), out var userId)
                                 ? userId
                                 : Guid.Empty;

    #endregion

    #region Private Methods

    protected void SetRequest(IRequestBase request)
    {
        request.UserId = UserId;
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
