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
        }

        #endregion
    }
}
