using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheResistanceOnline.Data.Entities;

namespace TheResistanceOnline.Data.Configurations;

public class RoleClaimConfiguration: IEntityTypeConfiguration<RoleClaim>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<RoleClaim> builder)
    {
        builder.ToTable("RoleClaims");

        builder.HasKey(e => e.Id);

        builder.Property(u => u.Id)
               .HasConversion(id => id.Value, value => new RoleClaimId(value))
               .ValueGeneratedOnAdd();

        // Each Role can have many associated RoleClaims
        builder.HasOne(e => e.Role)
               .WithMany(r => r.RoleClaims)
               .HasForeignKey(e => e.RoleId)
               .IsRequired();
    }

    #endregion
}
