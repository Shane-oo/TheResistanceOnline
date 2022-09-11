using TheResistanceOnline.Data.Core;

namespace TheResistanceOnline.Data
{
    public interface IDataContext
    {
        void Add(object entity);

        T Query<T>() where T : class, IDbQuery;

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
