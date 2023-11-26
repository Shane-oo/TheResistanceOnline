using Microsoft.EntityFrameworkCore;
using TheResistanceOnline.Data;
using TheResistanceOnline.Data.Entities;
using TheResistanceOnline.Data.Queries;

namespace TheResistanceOnline.Authentications.ExternalIdentities;

public interface IGoogleUserBySubjectDbQuery: IDbQuery<GoogleUser>
{
    IGoogleUserBySubjectDbQuery Include(params string[] include);

    IGoogleUserBySubjectDbQuery WithNoTracking();

    IGoogleUserBySubjectDbQuery WithParams(GoogleId googleId);
}

public class GoogleUserBySubjectDbQuery: IGoogleUserBySubjectDbQuery
{
    #region Fields

    private bool _asNoTracking;

    private readonly Context _context;

    private GoogleId _googleId;
    private string[] _include;

    #endregion

    #region Construction

    public GoogleUserBySubjectDbQuery(Context context)
    {
        _context = context;
    }

    #endregion

    #region Public Methods

    public async Task<GoogleUser> ExecuteAsync(CancellationToken cancellationToken)
    {
        var query = _context.GoogleUsers.Where(g => g.Id == _googleId);

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

    public IGoogleUserBySubjectDbQuery Include(params string[] include)
    {
        _include = include;
        return this;
    }

    public IGoogleUserBySubjectDbQuery WithNoTracking()
    {
        _asNoTracking = true;
        return this;
    }

    public IGoogleUserBySubjectDbQuery WithParams(GoogleId googleId)
    {
        _googleId = googleId;
        return this;
    }

    #endregion
}
