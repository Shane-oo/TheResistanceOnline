using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheResistanceOnline.Data.Entities;

namespace TheResistanceOnline.Data.Configurations;

public class PlayerStatisticConfiguration: IEntityTypeConfiguration<PlayerStatistic>
{
    public void Configure(EntityTypeBuilder<PlayerStatistic> builder)
    {
        builder.HasKey(us => us.Id);
        
        builder.Property(u => u.UserId)
               .HasConversion(id => id.Value, value => new UserId(value));

        builder.Property(ps => ps.UserId).IsRequired();
        builder.Property(ps => ps.Team).IsRequired();
        builder.Property(ps => ps.Won).IsRequired();

        builder.HasOne(ps => ps.Game)
               .WithMany(g => g.PlayerStatistics);

        builder.HasOne(ps => ps.User)
               .WithMany(u=>u.PlayerStatistics)
               .HasForeignKey(ps => ps.UserId)
               .IsRequired();
    }
}
