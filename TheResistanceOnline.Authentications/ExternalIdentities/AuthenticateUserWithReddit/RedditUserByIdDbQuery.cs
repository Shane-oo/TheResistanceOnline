using TheResistanceOnline.Data.Entities.Reddit;
using TheResistanceOnline.Data.Queries;

namespace TheResistanceOnline.Authentications.ExternalIdentities.AuthenticateUserWithReddit;

public interface IRedditUserByIdDbQuery: IDbQuery<RedditUser>
{
    IRedditUserByIdDbQuery WithParams(RedditId redditId);
}

public class RedditUserByIdDbQuery: IRedditUserByIdDbQuery
{
    public Task<RedditUser> ExecuteAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public IRedditUserByIdDbQuery WithParams(RedditId redditId)
    {
        throw new NotImplementedException();
    }
}
