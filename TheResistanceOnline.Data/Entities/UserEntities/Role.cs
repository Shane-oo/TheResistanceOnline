using Microsoft.AspNetCore.Identity;

namespace TheResistanceOnline.Data.Entities.UserEntities;

public class Role: IdentityRole<Guid>
{
    #region Properties

    public virtual ICollection<RoleClaim> RoleClaims { get; set; }

    public virtual ICollection<UserRole> UserRoles { get; set; }

    #endregion
}
