using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheResistanceOnline.Data.Entities;

namespace TheResistanceOnline.Data.Configurations;

public class MicrosoftUserConfiguration: IEntityTypeConfiguration<MicrosoftUser>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<MicrosoftUser> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
               .HasConversion(id => id.Value, value => new MicrosoftId(value))
               .ValueGeneratedNever();

        builder.Property(e => e.UserId)
               .IsRequired();

        builder.HasOne(m => m.User)
               .WithOne(u => u.MicrosoftUser)
               .HasForeignKey<MicrosoftUser>(m => m.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => new { e.Id, e.UserId })
               .IsUnique();
    }

    #endregion
}
