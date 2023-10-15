using Microsoft.AspNetCore.Identity;

namespace TheResistanceOnline.Data.Entities.UserEntities;

public class RoleClaim: IdentityRoleClaim<Guid>
{
    #region Properties

    public virtual Role Role { get; set; }

    #endregion
}
