using OpenIddict.EntityFrameworkCore.Models;

namespace TheResistanceOnline.Data.Entities;

public class Token: OpenIddictEntityFrameworkCoreToken<int, Application, Authorization>
{
    #region Properties

    public int? ApplicationId { get; set; }

    public int? AuthorizationId { get; set; }

    #endregion
}
