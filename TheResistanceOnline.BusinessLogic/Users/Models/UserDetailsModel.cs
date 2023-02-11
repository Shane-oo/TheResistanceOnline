using JetBrains.Annotations;
using TheResistanceOnline.BusinessLogic.PlayerStatistics.Models;

namespace TheResistanceOnline.BusinessLogic.Users.Models
{
    public class UserDetailsModel
    {
        #region Properties

        [NotNull]
        public string Email { get; set; }

        public PlayerStatisticDetailsModel PlayerStatistic { get; set; }

        //[CanBeNull]
        //public ProfilePicture ProfilePicture { get; set; }

        [NotNull]
        public string UserId { get; set; }

        [NotNull]
        public string UserName { get; set; }

        #endregion
    }
}
