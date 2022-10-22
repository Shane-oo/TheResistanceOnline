using JetBrains.Annotations;
using TheResistanceOnline.Data.Entities;

namespace TheResistanceOnline.Data.DiscordServer
{
    public class DiscordRole: NamedEntity<int>
    {
        #region Properties

        public int DiscordChannelId { get; set; }

        [NotNull]
        public DiscordChannel DiscordChannel { get; set; }

        #endregion
    }
}
