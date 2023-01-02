using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheResistanceOnline.Data.DiscordServer;
using TheResistanceOnline.Data.ProfilePictures;
using TheResistanceOnline.Data.Users;
using TheResistanceOnline.Data.UserSettings;

namespace TheResistanceOnline.Infrastructure.Data.Configurations.UsersConfigurations
{
    public class UserEntityConfiguration: IEntityTypeConfiguration<User>
    {
        #region Public Methods

        public void Configure(EntityTypeBuilder<User> builder)
        {
            //builder.HasOne<UserSetting>(u=>u.UserSetting).WithOne()
            builder.HasOne(u => u.UserSetting)
                   .WithOne(us => us.User)
                   .HasForeignKey<UserSetting>(us => us.UserId);

            builder.HasOne(u => u.DiscordUser)
                   .WithOne(du => du.User)
                   .HasForeignKey<DiscordUser>(du => du.UserId);

            builder.HasOne(u => u.ProfilePicture)
                   .WithOne(pp => pp.User)
                   .HasForeignKey<ProfilePicture>(pp => pp.UserId);

            builder.HasMany(u => u.PlayerStatistics)
                   .WithOne()
                   .HasForeignKey(ps => ps.UserId);
        }

        #endregion
    }
}
