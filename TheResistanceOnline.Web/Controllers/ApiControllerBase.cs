using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;

namespace TheResistanceOnline.Web.Controllers;

[ApiController]
public class ApiControllerBase: ControllerBase
{
    #region Properties

    protected Guid UserId => User.Identity is { IsAuthenticated: true }
                             && Guid.TryParse(User.FindFirstValue(OpenIddictConstants.Claims.Subject), out var userId)
                                 ? userId
                                 : Guid.Empty;

    #endregion
}
