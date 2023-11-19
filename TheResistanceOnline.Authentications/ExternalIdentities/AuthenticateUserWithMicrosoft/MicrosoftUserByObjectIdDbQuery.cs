using Microsoft.EntityFrameworkCore;
using TheResistanceOnline.Data;
using TheResistanceOnline.Data.Entities;
using TheResistanceOnline.Data.Queries;

namespace TheResistanceOnline.Authentications.ExternalIdentities;

public interface IMicrosoftUserByObjectIdDbQuery: IDbQuery<MicrosoftUser>
{
    IMicrosoftUserByObjectIdDbQuery Include(params string[] include);

    IMicrosoftUserByObjectIdDbQuery WithNoTracking();

    IMicrosoftUserByObjectIdDbQuery WithParams(MicrosoftId microsoftId);
}

public class MicrosoftUserByObjectIdDbQuery: IMicrosoftUserByObjectIdDbQuery
{
    #region Fields

    private bool _asNoTracking;

    private readonly Context _context;
    private string[] _include;

    private MicrosoftId _microsoftId;

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
        var query = _context.MicrosoftUsers.Where(m => m.Id == _microsoftId);

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

    public IMicrosoftUserByObjectIdDbQuery Include(params string[] include)
    {
        _include = include;
        return this;
    }

    public IMicrosoftUserByObjectIdDbQuery WithNoTracking()
    {
        _asNoTracking = true;
        return this;
    }

    public IMicrosoftUserByObjectIdDbQuery WithParams(MicrosoftId microsoftId)
    {
        _microsoftId = microsoftId;
        return this;
    }

    #endregion
}
