using Microsoft.Extensions.DependencyInjection;
using TheResistanceOnline.Data.Queries;

namespace TheResistanceOnline.Data;

public interface IDataContext
{
    void Add(object entity);

    Task AddAsync(object entity, CancellationToken cancellationToken);

    T Query<T>() where T : class, IDbQuery;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}

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
        ArgumentNullException.ThrowIfNull(entity);

        _context.Add(entity);
    }

    public async Task AddAsync(object entity, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(entity);

        await _context.AddAsync(entity, cancellationToken);
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
