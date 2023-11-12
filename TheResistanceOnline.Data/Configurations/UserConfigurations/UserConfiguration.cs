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

        builder.Property(u => u.UserName)
               .HasMaxLength(31);

        builder.Property(u => u.NormalizedUserName)
               .HasMaxLength(31);

        // Each User can have many UserClaims
        builder.HasMany(e => e.UserClaims)
               .WithOne(e => e.User)
               .HasForeignKey(uc => uc.UserId)
               .IsRequired();

        // Each User can have many UserLogins
        builder.HasMany(e => e.UserLogins)
               .WithOne(e => e.User)
               .HasForeignKey(ul => ul.UserId)
               .IsRequired();

        // Each User can have many UserTokens
        builder.HasMany(e => e.UserTokens)
               .WithOne(e => e.User)
               .HasForeignKey(ut => ut.UserId)
               .IsRequired();

        builder.HasOne(e => e.UserRole)
               .WithOne(e => e.User)
               .HasForeignKey<UserRole>(ur => ur.UserId)
               .IsRequired();

        builder.HasOne(u => u.UserSetting)
               .WithOne(us => us.User)
               .HasForeignKey<UserSetting>(us => us.UserId)
               .IsRequired();

        builder.HasMany(u => u.PlayerStatistics)
               .WithOne(ps => ps.User)
               .HasForeignKey(ps => ps.UserId)
               .IsRequired();

        builder.HasOne(e => e.MicrosoftUser)
               .WithOne(m => m.User)
               .HasForeignKey<MicrosoftUser>(m => m.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.GoogleUser)
               .WithOne(g => g.User)
               .HasForeignKey<GoogleUser>(g => g.UserId)
               .OnDelete(DeleteBehavior.Cascade);
    }

    #endregion
}
