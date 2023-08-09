using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheResistanceOnline.Data.Entities.UserEntities;

namespace TheResistanceOnline.Data.Configurations.UserConfigurations;

public class UserClaimConfiguration: IEntityTypeConfiguration<UserClaim>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<UserClaim> builder)
    {
        builder.ToTable("UserClaims");
    }

    #endregion
}