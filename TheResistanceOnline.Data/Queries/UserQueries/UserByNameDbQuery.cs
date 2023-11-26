using Microsoft.EntityFrameworkCore;
using TheResistanceOnline.Data.Entities;

namespace TheResistanceOnline.Data.Queries;

public interface IUserByNameDbQuery: IDbQuery<User>
{
    IUserByNameDbQuery AsNoTracking();

    IUserByNameDbQuery Include(string[] include);

    IUserByNameDbQuery WithParams(string normalizedUserName);
}

public class UserByNameDbQuery: IUserByNameDbQuery
{
    #region Fields

    private bool _asNoTracking;

    private readonly Context _context;
    private string[] _include;
    private string _normalizedUserName;

    #endregion

    #region Construction

    public UserByNameDbQuery(Context context)
    {
        _context = context;
    }

    #endregion

    #region Public Methods

    public IUserByNameDbQuery AsNoTracking()
    {
        _asNoTracking = true;
        return this;
    }

    public async Task<User> ExecuteAsync(CancellationToken cancellationToken)
    {
        var query = _context.Users.Where(u => u.NormalizedUserName == _normalizedUserName);
        if (_include != null)
        {
            query = _include.Aggregate(query, (current, expression) => current.Include(expression));
        }

        if (_asNoTracking)
        {
            query = query.AsNoTracking();
        }

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public IUserByNameDbQuery Include(string[] include)
    {
        _include = include;
        return this;
    }

    public IUserByNameDbQuery WithParams(string normalizedUserName)
    {
        _normalizedUserName = normalizedUserName;
        return this;
    }

    #endregion
}
