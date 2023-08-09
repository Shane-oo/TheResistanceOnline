using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheResistanceOnline.Data.Entities.UserEntities;

namespace TheResistanceOnline.Data.Configurations.UserConfigurations;

public class UserTokenConfiguration: IEntityTypeConfiguration<UserToken>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<UserToken> builder)
    {
        builder.ToTable("UserTokens");
    }

    #endregion
}
