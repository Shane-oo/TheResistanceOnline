using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using TheResistanceOnline.Data.DiscordServer;
using TheResistanceOnline.Data.Entities;
using TheResistanceOnline.Data.Games;
using TheResistanceOnline.Data.PlayerStatistics;
using TheResistanceOnline.Data.ProfilePictures;
using TheResistanceOnline.Data.UserSettings;

namespace TheResistanceOnline.Data.Users;

public class User: IdentityUser, IAuditableEntity
{
    #region Properties

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    [CanBeNull]
    public DiscordUser DiscordUser { get; set; }

    [CanBeNull]
    public ProfilePicture ProfilePicture { get; set; }

    [NotNull]
    public UserSetting UserSetting { get; set; }

    public List<PlayerStatistic> PlayerStatistics { get; set; }

    #endregion
}
