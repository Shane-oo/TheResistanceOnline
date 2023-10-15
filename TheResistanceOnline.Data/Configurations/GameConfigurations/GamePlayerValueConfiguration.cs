using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheResistanceOnline.Data.Entities.GameEntities;

namespace TheResistanceOnline.Data.Configurations.GameConfigurations;

public class GamePlayerValueConfiguration: IEntityTypeConfiguration<GamePlayerValue>
{
    public void Configure(EntityTypeBuilder<GamePlayerValue> builder)
    {
        builder.HasKey(us => us.Id)
               .HasName("PK_GamePlayerValues");
        builder.Property(gpv => gpv.GameId).IsRequired();
        builder.OwnsMany(gpv => gpv.PlayerValues, ownedNavigationBuilder => { ownedNavigationBuilder.ToJson(); });
    }
}
