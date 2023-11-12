using Microsoft.EntityFrameworkCore;
using TheResistanceOnline.Data.Entities;

namespace TheResistanceOnline.Data.Queries;

// probably do not need this query
public interface IUserByNameOrEmailDbQuery: IDbQuery<User>
{
    IUserByNameOrEmailDbQuery AsNoTracking();

    IUserByNameOrEmailDbQuery Include(string[] include);

    IUserByNameOrEmailDbQuery WithParams(string nameOrEmail);
}

public class UserByNameOrEmailDbQuery: IUserByNameOrEmailDbQuery
{
    #region Fields

    private bool _asNoTracking;

    private readonly Context _context;
    private string[] _include;
    private string _userNameOrEmail;

    #endregion

    #region Construction

    public UserByNameOrEmailDbQuery(Context context)
    {
        _context = context;
    }

    #endregion

    #region Public Methods

    public IUserByNameOrEmailDbQuery AsNoTracking()
    {
        _asNoTracking = true;
        return this;
    }

    public async Task<User?> ExecuteAsync(CancellationToken cancellationToken)
    {
        var query = _context.Set<User>().AsQueryable();
        if (_include != null)
        {
            query = _include.Aggregate(query, (current, expression) => current.Include(expression));
        }

        if (_asNoTracking)
        {
            query = query.AsNoTracking();
        }

        return await query.FirstOrDefaultAsync(u => u.Email == _userNameOrEmail || u.UserName == _userNameOrEmail,
                                               cancellationToken);
    }

    public IUserByNameOrEmailDbQuery Include(string[] include)
    {
        _include = include;
        return this;
    }

    public IUserByNameOrEmailDbQuery WithParams(string nameOrEmail)
    {
        _userNameOrEmail = nameOrEmail;
        return this;
    }

    #endregion
}
