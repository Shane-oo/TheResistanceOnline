using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheResistanceOnline.Data.Entities.UserEntities;

namespace TheResistanceOnline.Data.Configurations.UserConfigurations;

public class RoleClaimConfiguration: IEntityTypeConfiguration<RoleClaim>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<RoleClaim> builder)
    {
        builder.ToTable("RoleClaims");
    }

    #endregion
}
