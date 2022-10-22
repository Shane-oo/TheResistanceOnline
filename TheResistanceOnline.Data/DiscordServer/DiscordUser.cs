using JetBrains.Annotations;
using TheResistanceOnline.Data.Entities;
using TheResistanceOnline.Data.Users;

namespace TheResistanceOnline.Data.DiscordServer;

public class DiscordUser: NamedEntity<int>, IAuditableEntity
{
    #region Properties

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string UserId { get; set; }

    public User User { get; set; }

    public int? DiscordRoleId { get; set; }

    [CanBeNull]
    public DiscordRole DiscordRole { get; set; }

    [NotNull]
    public string Discriminator { get; set; }

    [NotNull]
    // Username + Discriminator
    public string DiscordTag { get; set; }

    #endregion
}
