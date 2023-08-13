using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using TheResistanceOnline.Data.Entities.ExternalIdentitiesEntities;
using TheResistanceOnline.Data.PlayerStatistics;

namespace TheResistanceOnline.Data.Entities.UserEntities;

public class User: IdentityUser<Guid>, IAuditableEntity
{
    #region Properties

    public DateTimeOffset CreatedOn { get; set; }

    public DateTimeOffset? ModifiedOn { get; set; }

    public DateTimeOffset LoginOn { get; set; }

    public UserSetting UserSetting { get; set; }

    public MicrosoftUser MicrosoftUser { get; set; }

    public List<PlayerStatistic> PlayerStatistics { get; set; }

    public virtual ICollection<UserClaim> UserClaims { get; set; }

    public virtual ICollection<UserLogin> UserLogins { get; set; }

    public virtual ICollection<UserToken> UserTokens { get; set; }

    public virtual UserRole UserRole { get; set; }

    #endregion
}
