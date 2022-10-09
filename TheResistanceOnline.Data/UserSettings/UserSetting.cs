using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;
using TheResistanceOnline.Data.Users;

namespace TheResistanceOnline.Data.UserSettings
{
    [UsedImplicitly]
    [Table("UserSettings")]
    public class UserSetting
    {
        #region Properties

        public int Id { get; set; }

        public bool ResetPasswordLinkSent { get; set; }

        public DateTimeOffset? ResetPasswordLinkSentRecord { get; set; }

        public User User { get; set; }

        public string UserId { get; set; }

        public bool UserWantsToUseDiscord { get; set; }

        public DateTimeOffset? UserWantsToUseDiscordRecord { get; set; }

        #endregion
    }
}