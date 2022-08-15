using TheResistanceOnline.Data.ProfilePictures;

namespace TheResistanceOnline.BusinessLogic.Users.Models
{
    public class UserDetails
    {
        #region Properties

        public string? Email { get; set; }

        public ProfilePicture? ProfilePicture { get; set; }

        public string? UserName { get; set; }

        #endregion
    }
}
