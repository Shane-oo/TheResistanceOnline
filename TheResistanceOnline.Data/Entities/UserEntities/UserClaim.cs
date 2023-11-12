using Microsoft.AspNetCore.Identity;

namespace TheResistanceOnline.Data.Entities;

public class UserClaim: IdentityUserClaim<Guid>
{
    #region Properties

    public virtual User User { get; set; }

    #endregion
}
