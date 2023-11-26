using Microsoft.EntityFrameworkCore;
using TheResistanceOnline.Data.Entities;

namespace TheResistanceOnline.Data.Queries;

public interface IUserByUserIdDbQuery: IDbQuery<User>
{
    IUserByUserIdDbQuery Include(params string[] include);

    IUserByUserIdDbQuery WithNoTracking();

    IUserByUserIdDbQuery WithParams(UserId userId);
}

public class UserByUserIdDbQuery: IUserByUserIdDbQuery
{
    #region Fields

    private bool _asNoTracking;

    private readonly Context _context;
    private string[] _include;
    private UserId _userId;

    #endregion

    #region Construction

    public UserByUserIdDbQuery(Context context)
    {
        _context = context;
    }

    #endregion

    #region Public Methods

    public async Task<User> ExecuteAsync(CancellationToken cancellationToken)
    {
        var query = _context.Users.Where(u => u.Id == _userId);

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

    public IUserByUserIdDbQuery Include(params string[] include)
    {
        _include = include;
        return this;
    }

    public IUserByUserIdDbQuery WithNoTracking()
    {
        _asNoTracking = true;
        return this;
    }

    public IUserByUserIdDbQuery WithParams(UserId userId)
    {
        _userId = userId;
        return this;
    }

    #endregion
}
