using JetBrains.Annotations;
using TheResistanceOnline.BusinessLogic.DiscordServer.Models;

namespace TheResistanceOnline.BusinessLogic.Users.Models
{
    public class UserDetailsModel
    {
        #region Properties

        [CanBeNull]
        public DiscordUserDetailsModel DiscordUser { get; set; }

        [NotNull]
        public string Email { get; set; }

        //[CanBeNull]
        //public ProfilePicture ProfilePicture { get; set; }

        [NotNull]
        public string UserId { get; set; }

        [NotNull]
        public string UserName { get; set; }

        #endregion
    }
}
