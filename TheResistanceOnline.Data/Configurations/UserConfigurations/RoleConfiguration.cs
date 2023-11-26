using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheResistanceOnline.Data.Entities;

namespace TheResistanceOnline.Data.Configurations;

public class RoleConfiguration: IEntityTypeConfiguration<Role>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");

        builder.HasKey(r => r.Id);

        builder.Property(u => u.Id)
               .HasConversion(id => id.Value, value => new RoleId(value))
               .ValueGeneratedOnAdd();

        builder.Property(r => r.ConcurrencyStamp).IsConcurrencyToken();
        builder.Property(u => u.Name).HasMaxLength(256);
        builder.Property(u => u.NormalizedName).HasMaxLength(256);

        builder.HasIndex(r => r.NormalizedName).HasDatabaseName("RoleNameIndex").IsUnique();
    }

    #endregion
}
