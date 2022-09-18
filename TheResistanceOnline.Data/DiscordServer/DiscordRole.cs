using JetBrains.Annotations;

namespace TheResistanceOnline.Data.DiscordServer
{
    public class DiscordRole
    {
        #region Properties

        [NotNull]
        public DiscordChannel DiscordChannel { get; set; }

        [NotNull]
        public int DiscordChannelId { get; set; }


        public int Id { get; set; }

        [NotNull]
        public string Name { get; set; }

        #endregion
    }
}
