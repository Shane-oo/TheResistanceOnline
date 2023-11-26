using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheResistanceOnline.Data.Entities;

namespace TheResistanceOnline.Data.Configurations;

public class RedditUserConfiguration: IEntityTypeConfiguration<RedditUser>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<RedditUser> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
               .HasConversion(id => id.Value, value => new RedditId(value))
               .ValueGeneratedNever();

        builder.Property(r => r.UserId)
               .IsRequired();

        builder.HasOne(r => r.User)
               .WithOne(u => u.RedditUser)
               .HasForeignKey<RedditUser>(r => r.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(r => new { r.Id, r.UserId })
               .IsUnique();
    }

    #endregion
}
