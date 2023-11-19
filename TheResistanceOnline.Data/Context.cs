using Microsoft.EntityFrameworkCore;
using TheResistanceOnline.Data.Configurations;
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

        // Users
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new RoleClaimConfiguration());
        modelBuilder.ApplyConfiguration(new UserClaimConfiguration());
        modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
        // External Identities
        modelBuilder.ApplyConfiguration(new MicrosoftUserConfiguration());
        modelBuilder.ApplyConfiguration(new GoogleUserConfiguration());
        // Games
        modelBuilder.ApplyConfiguration(new GameConfiguration());
        modelBuilder.ApplyConfiguration(new GamePlayerValueConfiguration());
        // Player Statistics
        modelBuilder.ApplyConfiguration(new PlayerStatisticConfiguration());
        // Authorizations
        modelBuilder.ApplyConfiguration(new ApplicationConfiguration());
        modelBuilder.ApplyConfiguration(new AuthorizationConfiguration());
        modelBuilder.ApplyConfiguration(new TokenConfiguration());
        modelBuilder.ApplyConfiguration(new ScopeConfiguration());
    }

    #endregion
}
