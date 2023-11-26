using Microsoft.EntityFrameworkCore;
using TheResistanceOnline.Data;
using TheResistanceOnline.Data.Entities;
using TheResistanceOnline.Data.Queries;

namespace TheResistanceOnline.Authentications.ExternalIdentities;

public interface IRedditUserByIdDbQuery: IDbQuery<RedditUser>
{
    IRedditUserByIdDbQuery WithNoTracking();

    IRedditUserByIdDbQuery WithParams(RedditId redditId);
}

public class RedditUserByIdDbQuery: IRedditUserByIdDbQuery
{
    #region Fields

    private bool _asNoTracking;

    private readonly Context _context;
    private RedditId _redditId;

    #endregion

    #region Construction

    public RedditUserByIdDbQuery(Context context)
    {
        _context = context;
    }

    #endregion

    #region Public Methods

    public async Task<RedditUser> ExecuteAsync(CancellationToken cancellationToken)
    {
        var query = _context.RedditUsers.Where(r => r.Id == _redditId);

        if (_asNoTracking)
        {
            query = query.AsNoTracking();
        }

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public IRedditUserByIdDbQuery WithNoTracking()
    {
        _asNoTracking = true;
        return this;
    }

    public IRedditUserByIdDbQuery WithParams(RedditId redditId)
    {
        _redditId = redditId;
        return this;
    }

    #endregion
}
