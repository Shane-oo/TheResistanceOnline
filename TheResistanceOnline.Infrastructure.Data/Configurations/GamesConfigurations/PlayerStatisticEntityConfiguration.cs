using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheResistanceOnline.Data.Games;

namespace TheResistanceOnline.Infrastructure.Data.Configurations.GamesConfigurations;

public class PlayerStatisticEntityConfiguration: IEntityTypeConfiguration<PlayerStatistic>
{
    public void Configure(EntityTypeBuilder<PlayerStatistic> builder)
    {
        builder.HasKey(us => us.Id)
               .HasName("PK_PlayerStatistics");

        builder.HasOne(ps => ps.Game)
               .WithMany(g => g.PlayerStatistics);

        builder.Property(ps => ps.UserId).IsRequired(false).HasMaxLength(450);
        builder.Property(ps => ps.PlayerId).IsRequired().HasMaxLength(450);
        builder.Property(ps => ps.Team).IsRequired();
    }
}
