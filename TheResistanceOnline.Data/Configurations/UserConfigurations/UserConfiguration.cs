using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheResistanceOnline.Data.Entities.UserEntities;
using TheResistanceOnline.Data.UserSettings;

namespace TheResistanceOnline.Data.Configurations.UserConfigurations;

public class UserConfiguration: IEntityTypeConfiguration<User>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasOne(u => u.UserSetting)
               .WithOne(us => us.User)
               .HasForeignKey<UserSetting>(us => us.UserId);

        builder.HasMany(u => u.PlayerStatistics)
               .WithOne()
               .HasForeignKey(ps => ps.UserId);
    }

    #endregion
}
