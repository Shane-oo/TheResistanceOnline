using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheResistanceOnline.Data.DiscordServer;

namespace TheResistanceOnline.Infrastructure.Data.Configurations.DiscordServerConfigurations
{
    public class DiscordRoleConfiguration: IEntityTypeConfiguration<DiscordRole>

    {
        #region Public Methods

        public void Configure(EntityTypeBuilder<DiscordRole> builder)
        {
            builder.HasKey(r => r.Id)
                   .HasName("PK_DiscordRoles");
            builder.HasData(new DiscordRole
                            {
                                Id = 1,
                                Name = "Can Join Game-1",
                                DiscordChannelId = 1
                            },
                            new DiscordRole
                            {
                                Id = 2,
                                Name = "Can Join Game-2",
                                DiscordChannelId = 2
                            },
                            new DiscordRole
                            {
                                Id = 3,
                                Name = "Can Join Game-3",
                                DiscordChannelId = 3
                            },
                            new DiscordRole
                            {
                                Id = 4,
                                Name = "Can Join Game-4",
                                DiscordChannelId = 4
                            },
                            new DiscordRole
                            {
                                Id = 5,
                                Name = "Can Join Game-5",
                                DiscordChannelId = 5
                            },
                            new DiscordRole
                            {
                                Id = 6,
                                Name = "Can Join Game-6",
                                DiscordChannelId = 6
                            },
                            new DiscordRole
                            {
                                Id = 7,
                                Name = "Can Join Game-7",
                                DiscordChannelId = 7
                            },
                            new DiscordRole
                            {
                                Id = 8,
                                Name = "Can Join Game-8",
                                DiscordChannelId = 8
                            },
                            new DiscordRole
                            {
                                Id = 9,
                                Name = "Can Join Game-9",
                                DiscordChannelId = 9
                            },
                            new DiscordRole
                            {
                                Id = 10,
                                Name = "Can Join Game-10",
                                DiscordChannelId = 10
                            });
        }

        #endregion
    }
}
