using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheResistanceOnline.Data.DiscordUsers;

namespace TheResistanceOnline.Infrastructure.Data.Configurations.DiscordUsersConfigurations
{
    public class DiscordUserConfiguration: IEntityTypeConfiguration<DiscordUser>

    {
        #region Public Methods

        public void Configure(EntityTypeBuilder<DiscordUser> builder)
        {
            builder.HasKey(d => d.Id)
                   .HasName("PK_DiscordUsers");
        }

        #endregion
    }
}
