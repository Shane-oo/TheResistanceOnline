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

        builder.Property(e => e.ObjectId)
               .IsRequired();

        builder.Property(e => e.UserId)
               .IsRequired();

        builder.HasIndex(e => new { e.ObjectId, e.UserId })
               .IsUnique();
    }

    #endregion
}
