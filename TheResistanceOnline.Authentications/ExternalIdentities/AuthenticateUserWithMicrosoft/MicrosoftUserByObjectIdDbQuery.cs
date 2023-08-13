using Microsoft.EntityFrameworkCore;
using TheResistanceOnline.Data;
using TheResistanceOnline.Data.Entities.ExternalIdentitiesEntities;
using TheResistanceOnline.Data.Queries;

namespace TheResistanceOnline.Authentications.ExternalIdentities.AuthenticateUserWithMicrosoft;

public interface IMicrosoftUserByObjectIdDbQuery: IDbQuery<MicrosoftUser>
{
    IMicrosoftUserByObjectIdDbQuery Include(params string[] include);

    IMicrosoftUserByObjectIdDbQuery WithParams(Guid objectId);
}

public class MicrosoftUserByObjectIdDbQuery: IMicrosoftUserByObjectIdDbQuery
{
    #region Fields

    private readonly Context _context;
    private string[] _include;

    private Guid _objectId;

    #endregion

    #region Construction

    public MicrosoftUserByObjectIdDbQuery(Context context)
    {
        _context = context;
    }

    #endregion

    #region Public Methods

    public async Task<MicrosoftUser> ExecuteAsync(CancellationToken cancellationToken)
    {
        var query = _context.MicrosoftUsers.Where(m => m.ObjectId.Equals(_objectId));
        if (_include != null)
        {
            query = _include.Aggregate(query, (current, expression) => current.Include(expression));
        }

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public IMicrosoftUserByObjectIdDbQuery Include(params string[] include)
    {
        _include = include;
        return this;
    }

    public IMicrosoftUserByObjectIdDbQuery WithParams(Guid objectId)
    {
        _objectId = objectId;
        return this;
    }

    #endregion
}
