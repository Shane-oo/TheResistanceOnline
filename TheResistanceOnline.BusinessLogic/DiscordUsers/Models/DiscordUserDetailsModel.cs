using JetBrains.Annotations;

namespace TheResistanceOnline.BusinessLogic.DiscordUsers.Models
{
    public class DiscordUserDetailsModel
    {
        #region Properties

        [CanBeNull]
        public string UserName { get; set; }

        #endregion
    }
}
