using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheResistanceOnline.Data.Entities.UserEntities;
using TheResistanceOnline.Data.UserSettings;

namespace TheResistanceOnline.Data.Configurations.UserConfigurations;

public class UserConfiguration: IEntityTypeConfiguration<User>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

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

        // Each User can have many entries in the UserRole join table
        builder.HasMany(e => e.UserRoles)
               .WithOne(e => e.User)
               .HasForeignKey(ur => ur.UserId)
               .IsRequired();

        builder.HasOne(u => u.UserSetting)
               .WithOne(us => us.User)
               .HasForeignKey<UserSetting>(us => us.UserId);

        builder.HasMany(u => u.PlayerStatistics)
               .WithOne()
               .HasForeignKey(ps => ps.UserId);
    }

    #endregion
}
