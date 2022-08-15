using Microsoft.AspNetCore.Identity;
using TheResistanceOnline.Data.ProfilePictures;

namespace TheResistanceOnline.Data.Users;

public class User: IdentityUser
{
    #region Properties

    public ProfilePicture ProfilePicture { get; set; }

    public int? ProfilePictureId { get; set; }

    #endregion
}
