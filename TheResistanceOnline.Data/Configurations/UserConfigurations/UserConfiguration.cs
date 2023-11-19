using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheResistanceOnline.Data.Entities;


namespace TheResistanceOnline.Data.Configurations;

public class UserConfiguration: IEntityTypeConfiguration<User>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
               .HasConversion(id => id.Value, value => new UserId(value))
               .ValueGeneratedOnAdd();

        builder.Property(u => u.UserName)
               .HasMaxLength(31);

        builder.Property(u => u.NormalizedUserName)
               .HasMaxLength(31);

        builder.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();

        builder.HasIndex(u => u.NormalizedUserName).HasDatabaseName("UserNameIndex").IsUnique();
    }

    #endregion
}
