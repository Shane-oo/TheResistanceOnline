using Microsoft.EntityFrameworkCore;
using TheResistanceOnline.Data.Entities;

namespace TheResistanceOnline.Data.Queries;

public interface IRoleByNameDbQuery: IDbQuery<Role>
{
    IRoleByNameDbQuery WithParams(string normalizedName);
}

public class RoleByNameDbQuery: IRoleByNameDbQuery
{
    #region Fields

    private readonly Context _context;
    private string _normalizedName;

    #endregion

    #region Construction

    public RoleByNameDbQuery(Context context)
    {
        _context = context;
    }

    #endregion

    #region Public Methods

    public async Task<Role> ExecuteAsync(CancellationToken cancellationToken)
    {

        return await _context.Roles.Where(r => r.NormalizedName == _normalizedName)
                             .FirstOrDefaultAsync(cancellationToken);
    }

    public IRoleByNameDbQuery WithParams(string normalizedName)
    {
        _normalizedName = normalizedName;
        return this;
    }

    #endregion
}
