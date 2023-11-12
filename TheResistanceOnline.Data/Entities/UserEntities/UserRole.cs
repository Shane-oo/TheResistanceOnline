using Microsoft.AspNetCore.Identity;

namespace TheResistanceOnline.Data.Entities;

public class UserRole: IdentityUserRole<Guid>
{
    #region Properties

    public virtual Role Role { get; set; }

    public virtual User User { get; set; }

    #endregion
}
