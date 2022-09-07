using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TheResistanceOnline.Data.DiscordUsers;
using TheResistanceOnline.Data.ProfilePictures;
using TheResistanceOnline.Data.Users;
using TheResistanceOnline.Infrastructure.Data.Configurations.DiscordUsersConfigurations;
using TheResistanceOnline.Infrastructure.Data.Configurations.ProfilePicturesConfigurations;

namespace TheResistanceOnline.Infrastructure.Data;

public class Context: IdentityDbContext<User>
{
    #region Properties

    public DbSet<DiscordUser> DiscordUsers { get; set; }

    public DbSet<ProfilePicture> ProfilePictures { get; set; }

    #endregion

    #region Construction

    public Context(DbContextOptions options): base(options)
    {
    }

    #endregion

    #region Private Methods

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        // modelBuilder.ApplyConfiguration(new UserRoleEntityConfiguration());
        modelBuilder.ApplyConfiguration(new ProfilePictureConfiguration());
        modelBuilder.ApplyConfiguration(new DiscordUserConfiguration());
    }

    #endregion
}
