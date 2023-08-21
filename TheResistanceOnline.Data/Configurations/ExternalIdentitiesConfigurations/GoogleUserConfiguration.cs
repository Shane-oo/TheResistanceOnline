using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheResistanceOnline.Data.Entities.ExternalIdentitiesEntities;

namespace TheResistanceOnline.Data.Configurations.ExternalIdentitiesConfigurations;

public class GoogleUserConfiguration: IEntityTypeConfiguration<GoogleUser>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<GoogleUser> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Subject)
               .IsRequired();

        builder.Property(e => e.UserId)
               .IsRequired();

        builder.HasIndex(e => new { e.Subject, e.UserId })
               .IsUnique();
    }

    #endregion
}
