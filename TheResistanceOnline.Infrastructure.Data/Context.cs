using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TheResistanceOnline.Data.ProfilePictures;
using TheResistanceOnline.Data.Users;
using TheResistanceOnline.Infrastructure.Data.Configurations.ProfilePicturesConfigurations;

namespace TheResistanceOnline.Infrastructure.Data;

public class Context: IdentityDbContext<User>
{
    #region Properties

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
    }

    #endregion
}
