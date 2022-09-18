using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TheResistanceOnline.Data.DiscordServer;
using TheResistanceOnline.Data.ProfilePictures;
using TheResistanceOnline.Data.Users;
using TheResistanceOnline.Infrastructure.Data.Configurations.DiscordServerConfigurations;
using TheResistanceOnline.Infrastructure.Data.Configurations.ProfilePicturesConfigurations;

namespace TheResistanceOnline.Infrastructure.Data;

public class Context: IdentityDbContext<User>
{
    #region Properties

    public DbSet<DiscordChannel> DiscordChannels { get; set; }

    public DbSet<DiscordRole> DiscordRoles { get; set; }

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
        modelBuilder.ApplyConfiguration(new DiscordChannelConfiguration());
        modelBuilder.ApplyConfiguration(new DiscordRoleConfiguration());
    }

    #endregion
}
