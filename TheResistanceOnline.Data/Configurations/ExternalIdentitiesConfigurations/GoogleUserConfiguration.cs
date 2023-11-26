using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheResistanceOnline.Data.Entities;

namespace TheResistanceOnline.Data.Configurations;

public class GoogleUserConfiguration: IEntityTypeConfiguration<GoogleUser>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<GoogleUser> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
               .HasConversion(id => id.Value, value => new GoogleId(value))
               .ValueGeneratedNever();

        builder.Property(e => e.Id)
               .HasMaxLength(255);

        builder.Property(e => e.UserId)
               .IsRequired();

        builder.HasOne(g => g.User)
               .WithOne(u => u.GoogleUser)
               .HasForeignKey<GoogleUser>(g => g.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => new { e.Id, e.UserId })
               .IsUnique();
    }

    #endregion
}
