using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheResistanceOnline.Data.Entities;

namespace TheResistanceOnline.Data.Configurations;

public class ApplicationConfiguration: IEntityTypeConfiguration<Application>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<Application> builder)
    {
        builder.ToTable("Applications");

        builder.Property(e => e.ClientId).HasMaxLength(100);
        builder.Property(e => e.ConcurrencyToken).HasMaxLength(50).IsConcurrencyToken();
        builder.Property(e => e.ConsentType).HasMaxLength(50);
        builder.Property(e => e.ClientType).HasMaxLength(50);

        builder.HasIndex(e => e.ClientId)
               .IsUnique();
    }

    #endregion
}
