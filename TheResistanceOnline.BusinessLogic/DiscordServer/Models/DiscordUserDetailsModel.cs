using JetBrains.Annotations;

namespace TheResistanceOnline.BusinessLogic.DiscordServer.Models
{
    [UsedImplicitly]
    public class DiscordUserDetailsModel
    {
        #region Properties

        [CanBeNull]
        public string UserName { get; set; }

        #endregion
    }
}
