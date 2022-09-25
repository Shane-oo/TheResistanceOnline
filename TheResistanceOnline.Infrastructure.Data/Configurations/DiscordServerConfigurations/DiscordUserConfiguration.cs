using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheResistanceOnline.Data.DiscordServer;

namespace TheResistanceOnline.Infrastructure.Data.Configurations.DiscordServerConfigurations
{
    public class DiscordUserConfiguration: IEntityTypeConfiguration<DiscordUser>

    {
        #region Public Methods

        public void Configure(EntityTypeBuilder<DiscordUser> builder)
        {
            builder.HasKey(u => u.Id)
                   .HasName("PK_DiscordUsers");
            builder.Property(u => u.UserName).IsRequired();
            builder.Property(u => u.DiscordTag).IsRequired();
            builder.Property(u => u.Discriminator).IsRequired();
        }

        #endregion
    }
}
