using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheResistanceOnline.Data.Entities;

namespace TheResistanceOnline.Data.Configurations;

public class UserClaimConfiguration: IEntityTypeConfiguration<UserClaim>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<UserClaim> builder)
    {
        builder.ToTable("UserClaims");

        builder.HasKey(uc => uc.Id);

        builder.Property(uc => uc.Id)
               .HasConversion(id => id.Value, intValue => new UserClaimId(intValue))
               .ValueGeneratedOnAdd();

        // Each User can have many UserClaims
        builder.HasOne(uc => uc.User)
               .WithMany(u => u.UserClaims)
               .HasForeignKey(uc => uc.UserId)
               .IsRequired();
    }

    #endregion
}
