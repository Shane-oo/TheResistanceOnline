using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheResistanceOnline.Data.Entities;

namespace TheResistanceOnline.Data.Configurations;

public class UserRoleConfiguration: IEntityTypeConfiguration<UserRole>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("UserRoles");

        builder.HasKey(r => new { r.UserId, r.RoleId });

        // Each User can only have one role
        builder.HasOne(e => e.User)
               .WithOne(u => u.UserRole)
               .HasForeignKey<UserRole>(e => e.UserId)
               .IsRequired();

        // Each Role can have many entries in the UserRole join table
        builder.HasOne(ur => ur.Role)
               .WithMany(e => e.UserRoles)
               .HasForeignKey(ur => ur.RoleId)
               .IsRequired();

        builder.HasIndex(e => new { e.UserId, e.RoleId }).IsUnique();
    }

    #endregion
}
