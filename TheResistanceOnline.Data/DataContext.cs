using Microsoft.Extensions.DependencyInjection;
using TheResistanceOnline.Data.Entities;
using TheResistanceOnline.Data.Queries;

namespace TheResistanceOnline.Data;

public interface IDataContext
{
    void Add(IEntity entity);

    Task AddAsync(IEntity entity, CancellationToken cancellationToken);

    void Attach(IEntity entity);

    T Query<T>() where T : class, IDbQuery;

    void Remove(IEntity entity);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    void Update(IEntity entity);
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

    public void Add(IEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        _context.Add(entity);
    }

    public async Task AddAsync(IEntity entity, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(entity);

        await _context.AddAsync(entity, cancellationToken);
    }

    public void Attach(IEntity entity)
    {
        _context.Attach(entity);
    }

    public T Query<T>() where T : class, IDbQuery
    {
        return _serviceProvider.GetRequiredService<T>();
    }

    public void Remove(IEntity entity)
    {
        _context.Remove(entity);
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }

    public void Update(IEntity entity)
    {
        _context.Update(entity);
    }

    #endregion
}
