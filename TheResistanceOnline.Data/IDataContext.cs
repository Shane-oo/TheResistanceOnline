using TheResistanceOnline.Data.Core;
using TheResistanceOnline.Data.Core.Queries;

namespace TheResistanceOnline.Data
{
    public interface IDataContext
    {
        void Add(object entity);

        T Query<T>() where T : class, IDbQuery;

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
