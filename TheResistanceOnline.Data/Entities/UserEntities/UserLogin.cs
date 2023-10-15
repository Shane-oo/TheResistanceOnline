using Microsoft.AspNetCore.Identity;

namespace TheResistanceOnline.Data.Entities.UserEntities;

public class UserLogin: IdentityUserLogin<Guid>
{
    #region Properties

    public virtual User User { get; set; }

    #endregion
}
