using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheResistanceOnline.Data.DiscordServer;

namespace TheResistanceOnline.Infrastructure.Data.Configurations.DiscordServerConfigurations
{
    public class DiscordChannelConfiguration: IEntityTypeConfiguration<DiscordChannel>

    {
        #region Public Methods

        public void Configure(EntityTypeBuilder<DiscordChannel> builder)
        {
            builder.HasKey(c => c.Id)
                   .HasName("PK_DiscordChannels");

            builder.HasData(new DiscordChannel
                            {
                                Id = 1,
                                Name = "game-1"
                            },
                            new DiscordChannel
                            {
                                Id = 2,
                                Name = "game-2"
                            },
                            new DiscordChannel
                            {
                                Id = 3,
                                Name = "game-3"
                            },
                            new DiscordChannel
                            {
                                Id = 4,
                                Name = "game-4"
                            },
                            new DiscordChannel
                            {
                                Id = 5,
                                Name = "game-5"
                            },
                            new DiscordChannel
                            {
                                Id = 6,
                                Name = "game-6"
                            },
                            new DiscordChannel
                            {
                                Id = 7,
                                Name = "game-7"
                            },
                            new DiscordChannel
                            {
                                Id = 8,
                                Name = "game-8"
                            },
                            new DiscordChannel
                            {
                                Id = 9,
                                Name = "game-9"
                            },
                            new DiscordChannel
                            {
                                Id = 10,
                                Name = "game-10"
                            });
        }

        #endregion
    }
}
