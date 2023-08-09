using Microsoft.AspNetCore.Identity;

namespace TheResistanceOnline.Data.Entities.UserEntities;

public class UserToken: IdentityUserToken<Guid>
{
    #region Properties

    public virtual User User { get; set; }

    #endregion
}
