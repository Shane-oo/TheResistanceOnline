using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheResistanceOnline.Data.Games;

namespace TheResistanceOnline.Infrastructure.Data.Configurations.GamesConfigurations;

public class GamePlayerValueEntityConfiguration: IEntityTypeConfiguration<GamePlayerValue>
{
    public void Configure(EntityTypeBuilder<GamePlayerValue> builder)
    {
        builder.HasKey(us => us.Id)
               .HasName("PK_GamePlayerValues");
        builder.Property(gpv => gpv.GameId).IsRequired();
    }
}
