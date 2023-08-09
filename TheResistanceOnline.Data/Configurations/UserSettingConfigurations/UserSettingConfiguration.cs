using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheResistanceOnline.Data.UserSettings;

namespace TheResistanceOnline.Data.Configurations.UserSettingConfigurations;

public class UserSettingConfiguration: IEntityTypeConfiguration<UserSetting>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<UserSetting> builder)
    {
        builder.HasKey(us => us.Id)
               .HasName("PK_UserSettings");
    }

    #endregion
}
