using OpenIddict.EntityFrameworkCore.Models;

namespace TheResistanceOnline.Data.Entities.AuthorizationEntities;

public class Authorization: OpenIddictEntityFrameworkCoreAuthorization<int, Application, Token>
{
    #region Properties

    public int? ApplicationId { get; set; }

    #endregion
}