using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheResistanceOnline.Data.Entities.AuthorizationEntities;

namespace TheResistanceOnline.Data.Configurations.AuthorizationConfigurations;

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

        builder.HasMany(e => e.Tokens)
               .WithOne(t => t.Authorization)
               .HasForeignKey(t => t.AuthorizationId)
               .IsRequired(false)
               .HasConstraintName("FK_Tokens_Authorizations_AuthorizationId");


        builder.HasIndex(e => new { e.ApplicationId, e.Status, e.Subject, e.Type });
    }

    #endregion
}
