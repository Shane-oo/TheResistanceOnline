using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheResistanceOnline.Data.Entities.AuthorizationEntities;

namespace TheResistanceOnline.Data.Configurations.AuthorizationConfigurations;

public class ApplicationConfiguration: IEntityTypeConfiguration<Application>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<Application> builder)
    {
        builder.ToTable("Applications");

        builder.Property(e => e.ClientId).HasMaxLength(100);
        builder.Property(e => e.ConcurrencyToken).HasMaxLength(50).IsConcurrencyToken();
        builder.Property(e => e.ConsentType).HasMaxLength(50);
        builder.Property(e => e.Type).HasMaxLength(50);

        builder.HasMany(e => e.Authorizations)
               .WithOne(a => a.Application)
               .HasForeignKey(a => a.ApplicationId)
               .IsRequired(false)
               .HasConstraintName("FK_Authorizations_Applications_ApplicationId");

        builder.HasMany(e => e.Tokens)
               .WithOne(t => t.Application)
               .HasForeignKey(t => t.ApplicationId)
               .IsRequired(false)
               .HasConstraintName("FK_Tokens_Applications_ApplicationId");

        builder.HasIndex(e => e.ClientId)
               .IsUnique();
    }

    #endregion
}
