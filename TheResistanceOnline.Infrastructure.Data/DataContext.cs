using Microsoft.Extensions.DependencyInjection;
using TheResistanceOnline.Data;
using TheResistanceOnline.Data.Core;
using TheResistanceOnline.Data.Core.Queries;

namespace TheResistanceOnline.Infrastructure.Data
{
    public class DataContext: IDataContext
    {
        #region Fields

        private readonly Context _context;
        private readonly IServiceProvider _serviceProvider;

        #endregion

        #region Construction

        public DataContext(Context context, IServiceProvider serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;
        }

        #endregion

        #region Public Methods

        public void Add(object entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            _context.Add(entity);
        }

        public T Query<T>() where T : class, IDbQuery
        {
            return _serviceProvider.GetRequiredService<T>();
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }

        #endregion
    }
}
