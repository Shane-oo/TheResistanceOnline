using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using TheResistanceOnline.Data.PlayerStatistics;
using TheResistanceOnline.Data.UserSettings;

namespace TheResistanceOnline.Data.Entities.UserEntities;

public class User: IdentityUser<Guid>, IAuditableEntity
{
    #region Properties

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }


    public UserSetting UserSetting { get; set; }

    public List<PlayerStatistic> PlayerStatistics { get; set; }

    public virtual ICollection<UserClaim> UserClaims { get; set; }

    public virtual ICollection<UserLogin> UserLogins { get; set; }

    public virtual ICollection<UserToken> UserTokens { get; set; }

    public virtual ICollection<UserRole> UserRoles { get; set; }

    #endregion
}
