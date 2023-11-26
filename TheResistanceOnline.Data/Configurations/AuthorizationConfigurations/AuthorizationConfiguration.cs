using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheResistanceOnline.Data.Entities;

namespace TheResistanceOnline.Data.Configurations;

public class AuthorizationConfiguration: IEntityTypeConfiguration<Authorization>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<Authorization> builder)
    {
        builder.ToTable("Authorizations");

        builder.Property(e => e.ConcurrencyToken).HasMaxLength(50).IsConcurrencyToken();
        builder.Property(e => e.Status).HasMaxLength(50);
        builder.Property(e => e.Subject).HasMaxLength(400);
        builder.Property(e => e.Type).HasMaxLength(50);

        builder.HasOne(e => e.Application)
               .WithMany(a => a.Authorizations)
               .HasForeignKey(e => e.ApplicationId)
               .IsRequired(false)
               .HasConstraintName("FK_Authorizations_Applications_ApplicationId");

        builder.HasIndex(e => new { e.ApplicationId, e.Status, e.Subject, e.Type });
    }

    #endregion
}
