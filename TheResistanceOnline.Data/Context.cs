using Microsoft.EntityFrameworkCore;
using TheResistanceOnline.Data.Entities;

namespace TheResistanceOnline.Data;

public class Context: DbContext
{
    #region Properties

    public DbSet<Application> Applications { get; set; }

    public DbSet<Authorization> Authorizations { get; set; }

    public DbSet<GamePlayerValue> GamePlayerValues { get; set; }

    public DbSet<Game> Games { get; set; }

    public DbSet<GoogleUser> GoogleUsers { get; set; }

    public DbSet<MicrosoftUser> MicrosoftUsers { get; set; }

    public DbSet<PlayerStatistic> PlayerStatistics { get; set; }

    public DbSet<RedditUser> RedditUsers { get; set; }

    public DbSet<RoleClaim> RoleClaims { get; set; }

    public DbSet<Role> Roles { get; set; }

    public DbSet<Scope> Scopes { get; set; }

    public DbSet<Token> Tokens { get; set; }

    public DbSet<UserClaim> UserClaims { get; set; }

    public DbSet<UserRole> UserRoles { get; set; }

    public DbSet<User> Users { get; set; }

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
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Context).Assembly);
    }

    #endregion
}
