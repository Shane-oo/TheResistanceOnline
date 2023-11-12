using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheResistanceOnline.Data.Entities;

namespace TheResistanceOnline.Data.Configurations;

public class UserSettingConfiguration: IEntityTypeConfiguration<UserSetting>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<UserSetting> builder)
    {
        builder.HasKey(us => us.Id);


        builder.Property(e => e.UserId)
               .IsRequired();

        builder.HasIndex(e => e.UserId)
               .IsUnique();
    }

    #endregion
}
