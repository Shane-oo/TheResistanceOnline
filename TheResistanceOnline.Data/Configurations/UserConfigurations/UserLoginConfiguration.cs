using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheResistanceOnline.Data.Entities.UserEntities;

namespace TheResistanceOnline.Data.Configurations.UserConfigurations;

public class UserLoginConfiguration: IEntityTypeConfiguration<UserLogin>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<UserLogin> builder)
    {
        builder.ToTable("UserLogins");
    }

    #endregion
}
