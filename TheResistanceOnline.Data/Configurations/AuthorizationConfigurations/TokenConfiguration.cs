using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheResistanceOnline.Data.Entities;

namespace TheResistanceOnline.Data.Configurations;

public class TokenConfiguration: IEntityTypeConfiguration<Token>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<Token> builder)
    {
        builder.ToTable("Tokens");

        builder.Property(e => e.ConcurrencyToken).HasMaxLength(50).IsConcurrencyToken();
        builder.Property(e => e.ReferenceId).HasMaxLength(100);
        builder.Property(e => e.Status).HasMaxLength(50);
        builder.Property(e => e.Subject).HasMaxLength(400);
        builder.Property(e => e.Type).HasMaxLength(50);

        builder.HasOne(e => e.Authorization)
               .WithMany(a => a.Tokens)
               .HasForeignKey(e => e.AuthorizationId)
               .IsRequired(false)
               .HasConstraintName("FK_Tokens_Authorizations_AuthorizationId");

        builder.HasOne(e => e.Application)
               .WithMany(a => a.Tokens)
               .HasForeignKey(e => e.ApplicationId)
               .IsRequired(false)
               .HasConstraintName("FK_Tokens_Applications_ApplicationId");

        builder.HasIndex(e => e.ReferenceId)
               .IsUnique();

        builder.HasIndex(e => new { e.ApplicationId, e.Status, e.Subject, e.Type });
    }

    #endregion
}
