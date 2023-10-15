using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheResistanceOnline.Data.PlayerStatistics;

namespace TheResistanceOnline.Data.Configurations.PlayerStatisticConfigurations;

public class PlayerStatisticConfiguration: IEntityTypeConfiguration<PlayerStatistic>
{
    public void Configure(EntityTypeBuilder<PlayerStatistic> builder)
    {
        builder.HasKey(us => us.Id);

        builder.HasOne(ps => ps.Game)
               .WithMany(g => g.PlayerStatistics);

        builder.Property(ps => ps.UserId).IsRequired();
        builder.Property(ps => ps.Team).IsRequired();
        builder.Property(ps => ps.Won).IsRequired();
    }
}
