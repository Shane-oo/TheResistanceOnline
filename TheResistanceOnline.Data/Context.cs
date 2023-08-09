using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TheResistanceOnline.Data.Configurations.GameConfigurations;
using TheResistanceOnline.Data.Configurations.PlayerStatisticConfigurations;
using TheResistanceOnline.Data.Entities.GameEntities;
using TheResistanceOnline.Data.Entities.UserEntities;
using TheResistanceOnline.Data.PlayerStatistics;

namespace TheResistanceOnline.Data;

public class Context: IdentityDbContext<User>
{
    #region Properties

    public DbSet<GamePlayerValue> GamePlayerValues { get; set; }

    public DbSet<Game> Games { get; set; }

    public DbSet<PlayerStatistic> PlayerStatistics { get; set; }


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


        modelBuilder.ApplyConfiguration(new GameConfiguration());
        modelBuilder.ApplyConfiguration(new GamePlayerValueConfiguration());
        modelBuilder.ApplyConfiguration(new PlayerStatisticConfiguration());
    }

    #endregion
}
