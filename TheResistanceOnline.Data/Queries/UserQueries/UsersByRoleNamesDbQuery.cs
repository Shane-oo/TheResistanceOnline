using Microsoft.EntityFrameworkCore;
using TheResistanceOnline.Data.Entities;

namespace TheResistanceOnline.Data.Queries;

public interface IUsersByRoleNamesDbQuery: IDbQuery<List<User>>
{
    IUsersByRoleNamesDbQuery WithParams(string roleName);
}

public class UsersByRoleNamesDbQuery: IUsersByRoleNamesDbQuery
{
    #region Fields

    private readonly Context _context;

    private string _roleName;

    #endregion

    #region Construction

    public UsersByRoleNamesDbQuery(Context context)
    {
        _context = context;
    }

    #endregion

    #region Public Methods

    public async Task<List<User>> ExecuteAsync(CancellationToken cancellationToken)
    {
        return await _context.UserRoles.Where(ur => ur.Role.Name.Equals(_roleName))
                             .Select(u => u.User)
                             .AsNoTracking()
                             .ToListAsync(cancellationToken);
    }

    public IUsersByRoleNamesDbQuery WithParams(string roleName)
    {
        _roleName = roleName;
        return this;
    }

    #endregion
}
