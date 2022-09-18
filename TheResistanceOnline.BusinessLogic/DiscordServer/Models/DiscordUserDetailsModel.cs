using JetBrains.Annotations;
using TheResistanceOnline.Data.DiscordServer;

namespace TheResistanceOnline.BusinessLogic.DiscordServer.Models
{
    [UsedImplicitly]
    public class DiscordUserDetailsModel
    {
        #region Properties

        [CanBeNull]
        public DiscordRole DiscordRole { get; set; }

        // Username + Discriminator
        [NotNull]
        public string DiscordTag { get; set; }

        [NotNull]
        public string Discriminator { get; set; }

        [NotNull]
        public string UserName { get; set; }

        #endregion
    }
}
