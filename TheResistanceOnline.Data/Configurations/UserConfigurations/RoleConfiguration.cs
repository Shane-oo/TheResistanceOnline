using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheResistanceOnline.Data.Entities.UserEntities;

namespace TheResistanceOnline.Data.Configurations.UserConfigurations;

public class RoleConfiguration: IEntityTypeConfiguration<Role>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");

        // Each Role can have many entries in the UserRole join table
        builder.HasMany(e => e.UserRoles)
               .WithOne(e => e.Role)
               .HasForeignKey(ur => ur.RoleId)
               .IsRequired();

        // Each Role can have many associated RoleClaims
        builder.HasMany(e => e.RoleClaims)
               .WithOne(e => e.Role)
               .HasForeignKey(rc => rc.RoleId)
               .IsRequired();
    }

    #endregion
}
