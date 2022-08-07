using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TheResistanceOnline.Data.Users;
using TheResistanceOnline.Infrastructure.Data.Configurations;

namespace TheResistanceOnline.Infrastructure.Data;

public class Context: IdentityDbContext<User>
{
    #region Construction

    public Context(DbContextOptions options): base(options)
    {
    }

    #endregion

    #region Private Methods

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new UserRoleEntityConfiguration());
    }

    #endregion

    //public DbSet<object> objects {get;set;}
}
