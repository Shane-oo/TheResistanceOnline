using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace TheResistanceOnline.Infrastructure.Data.Migrations
{
    [UsedImplicitly]
    public class DesignTimeContextFactory: IDesignTimeDbContextFactory<Context>
    {
        #region Public Methods

        public Context CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("local.settings.json", true, true)
                                                          .AddEnvironmentVariables()
                                                          .Build();

            var connectionString = configuration.GetConnectionString("resistanceDb");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string was not set in configuration");
            }

            var builder = new DbContextOptionsBuilder<Context>().UseSqlServer(connectionString, b =>
                                                                                                {
                                                                                                    b.MigrationsAssembly(GetType().Assembly.GetName().Name);
                                                                                                    b.CommandTimeout(1800);
                                                                                                });
            return new Context(builder.Options);
        }

        #endregion
    }
}
