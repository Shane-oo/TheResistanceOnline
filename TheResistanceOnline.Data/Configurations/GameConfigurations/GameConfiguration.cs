using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheResistanceOnline.Data.Entities;

namespace TheResistanceOnline.Data.Configurations;

public class GameConfiguration: IEntityTypeConfiguration<Game>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder.HasKey(us => us.Id)
               .HasName("PK_Games");

        builder.HasMany(g => g.PlayerStatistics)
               .WithOne()
               .HasForeignKey(ps => ps.GameId);

        builder.HasOne(g => g.GamePlayerValue)
               .WithOne(gpv => gpv.Game)
               .HasForeignKey<GamePlayerValue>(gpv => gpv.GameId);

        builder.Property(g => g.WinningTeam).IsRequired();
    }

    #endregion
}
