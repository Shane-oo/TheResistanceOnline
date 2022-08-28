using TheResistanceOnline.Data.ProfilePictures;

namespace TheResistanceOnline.BusinessLogic.Users.Models
{
    public class UserDetailsModel
    {
        #region Properties

        public string? Email { get; set; }

        public ProfilePicture? ProfilePicture { get; set; }

        public string? UserName { get; set; }
        
        public string? UserId { get; set; }

        #endregion
    }
}
