using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using TheResistanceOnline.Data.PlayerStatistics;

using TheResistanceOnline.Data.UserSettings;

namespace TheResistanceOnline.Data.Entities.UserEntities;

public class User: IdentityUser, IAuditableEntity
{
    #region Properties

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }


    public UserSetting UserSetting { get; set; }

    public List<PlayerStatistic> PlayerStatistics { get; set; }

    #endregion
}
