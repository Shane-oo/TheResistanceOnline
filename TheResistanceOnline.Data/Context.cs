using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TheResistanceOnline.Data.Configurations.AuthorizationConfigurations;
using TheResistanceOnline.Data.Configurations.ExternalIdentitiesConfigurations;
using TheResistanceOnline.Data.Configurations.GameConfigurations;
using TheResistanceOnline.Data.Configurations.PlayerStatisticConfigurations;
using TheResistanceOnline.Data.Configurations.UserConfigurations;
using TheResistanceOnline.Data.Entities.AuthorizationEntities;
using TheResistanceOnline.Data.Entities.ExternalIdentitiesEntities;
using TheResistanceOnline.Data.Entities.GameEntities;
using TheResistanceOnline.Data.Entities.UserEntities;
using TheResistanceOnline.Data.PlayerStatistics;

namespace TheResistanceOnline.Data;

public class Context: IdentityDbContext<User, Role, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
{
    #region Properties

    public DbSet<Application> Applications { get; set; }

    public DbSet<Authorization> Authorizations { get; set; }

    public DbSet<GamePlayerValue> GamePlayerValues { get; set; }

    public DbSet<Game> Games { get; set; }

    public DbSet<MicrosoftUser> MicrosoftUsers { get; set; }

    public DbSet<PlayerStatistic> PlayerStatistics { get; set; }

    public DbSet<Scope> Scopes { get; set; }

    public DbSet<Token> Tokens { get; set; }

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
        modelBuilder.ApplyConfiguration(new UserLoginConfiguration());
        modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
        modelBuilder.ApplyConfiguration(new UserTokenConfiguration());
        modelBuilder.ApplyConfiguration(new UserSettingConfiguration());
        // External Identities
        modelBuilder.ApplyConfiguration(new MicrosoftUserConfiguration());
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
