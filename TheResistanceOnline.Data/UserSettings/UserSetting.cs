using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;

namespace TheResistanceOnline.Data.UserSettings
{
    [UsedImplicitly]
    [Table("UserSettings")]
    public class UserSetting
    {
        #region Properties

        public int Id { get; set; }

        public bool UserWantsToUseDiscord { get; set; }

        public DateTimeOffset UserWantsToUseDiscordRecord { get; set; }

        #endregion
    }
}
