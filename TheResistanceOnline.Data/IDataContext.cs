using System.Diagnostics.CodeAnalysis;
using TheResistanceOnline.Data.Core;

namespace TheResistanceOnline.Data
{
    public interface IDataContext
    {
        void Add([NotNull] object entity);

        T Query<T>() where T : class, IDbQuery;

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
