using Microsoft.EntityFrameworkCore;
using TheResistanceOnline.Data;
using TheResistanceOnline.Data.Entities.ExternalIdentitiesEntities;
using TheResistanceOnline.Data.Queries;

namespace TheResistanceOnline.Authentications.ExternalIdentities.AuthenticateUserWithGoogle;

public interface IGoogleUserBySubjectDbQuery: IDbQuery<GoogleUser>
{
    IGoogleUserBySubjectDbQuery Include(params string[] include);

    IGoogleUserBySubjectDbQuery WithNoTracking();

    IGoogleUserBySubjectDbQuery WithParams(Guid subject);
}

public class GoogleUserBySubjectDbQuery: IGoogleUserBySubjectDbQuery
{
    #region Fields

    private bool _asNoTracking;

    private readonly Context _context;
    private string[] _include;

    private Guid _subject;

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
        var query = _context.GoogleUsers.Where(g => g.Subject.Equals(_subject));

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

    public IGoogleUserBySubjectDbQuery WithParams(Guid subject)
    {
        _subject = subject;
        return this;
    }

    #endregion
}
