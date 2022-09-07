using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using TheResistanceOnline.Data.DiscordUsers;
using TheResistanceOnline.Data.ProfilePictures;

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

    #endregion
}
