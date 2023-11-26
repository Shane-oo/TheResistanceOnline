using Microsoft.EntityFrameworkCore;
using TheResistanceOnline.Data.Entities;

namespace TheResistanceOnline.Data.Queries;

public interface IRoleByIdDbQuery: IDbQuery<Role>
{
    IRoleByIdDbQuery WithParams(RoleId roleId);
}

public class RoleByIdDbQuery: IRoleByIdDbQuery
{
    #region Fields

    private readonly Context _context;
    private RoleId _roleId;

    #endregion

    #region Construction

    public RoleByIdDbQuery(Context context)
    {
        _context = context;
    }

    #endregion

    #region Public Methods

    public async Task<Role> ExecuteAsync(CancellationToken cancellationToken)
    {
        var query = _context.Roles.Where(r => r.Id == _roleId);

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public IRoleByIdDbQuery WithParams(RoleId roleId)
    {
        _roleId = roleId;
        return this;
    }

    #endregion
}
