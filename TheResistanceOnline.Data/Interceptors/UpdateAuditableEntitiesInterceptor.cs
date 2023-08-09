using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TheResistanceOnline.Data.Entities;

namespace TheResistanceOnline.Data.Interceptors;

public sealed class UpdateAuditableEntitiesInterceptor: SaveChangesInterceptor
{
    #region Public Methods

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
                                                                          InterceptionResult<int> result,
                                                                          CancellationToken cancellationToken = new CancellationToken())
    {
        var dbContext = eventData.Context;
        if (dbContext == null)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        var entries = dbContext.ChangeTracker
                               .Entries<IAuditableEntity>();

        foreach(var entityEntry in entries)
        {
            if (entityEntry.State == EntityState.Added)
            {
                entityEntry.Property(a => a.CreatedOn).CurrentValue = DateTime.UtcNow;
            }

            if (entityEntry.State == EntityState.Modified)
            {
                entityEntry.Property(a => a.ModifiedOn).CurrentValue = DateTime.UtcNow;
            }
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    #endregion
}
