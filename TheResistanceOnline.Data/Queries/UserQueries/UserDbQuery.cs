using Microsoft.EntityFrameworkCore;
using TheResistanceOnline.Data.Entities.UserEntities;

namespace TheResistanceOnline.Data.Queries.UserQueries;

public interface IUserDbQuery: IDbQuery<User>
{
    IUserDbQuery WithParams(string userId);
}

public class UserDbQuery: IUserDbQuery
{
    #region Fields

    private readonly Context _context;
    private string _userId;

    #endregion

    #region Construction

    public UserDbQuery(Context context)
    {
        _context = context;
    }

    #endregion

    #region Public Methods

    public async Task<User> ExecuteAsync(CancellationToken cancellationToken)
    {
        var user = await _context.Users
                                 .Include(us => us.UserSetting)
                                 .FirstAsync(u => u.Id == _userId,
                                             cancellationToken);
        return user;
    }

    public IUserDbQuery WithParams(string userId)
    {
        _userId = userId;
        return this;
    }

    #endregion
}
