using JetBrains.Annotations;
using TheResistanceOnline.Data.Users;

namespace TheResistanceOnline.Data.DiscordServer
{
    public class DiscordUser
    {
        #region Properties

        [CanBeNull]
        public DiscordRole DiscordRole { get; set; }

        public int? DiscordRoleId { get; set; }

        // Username + Discriminator
        [NotNull]
        public string DiscordTag { get; set; }

        [NotNull]
        public string Discriminator { get; set; }

        public int Id { get; set; }

        public User User { get; set; }

        public string UserId { get; set; }

        [NotNull]
        public string UserName { get; set; }

        #endregion
    }
}
