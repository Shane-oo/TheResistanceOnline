using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using TheResistanceOnline.Data.DiscordServer;
using TheResistanceOnline.Data.ProfilePictures;
using TheResistanceOnline.Data.UserSettings;

namespace TheResistanceOnline.Data.Users;

public class User: IdentityUser
{
    #region Properties

    [CanBeNull]
    public DiscordUser DiscordUser { get; set; }

    public int? DiscordUserId { get; set; }

    [CanBeNull]
    public ProfilePicture ProfilePicture { get; set; }

    public int? ProfilePictureId { get; set; }

    [NotNull]
    public UserSetting UserSetting { get; set; }

    public int UserSettingId { get; set; }

    #endregion
}
