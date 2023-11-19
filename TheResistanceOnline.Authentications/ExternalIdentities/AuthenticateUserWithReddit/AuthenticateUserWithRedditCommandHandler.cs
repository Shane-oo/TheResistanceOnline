using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using TheResistanceOnline.Core;
using TheResistanceOnline.Core.NewCommandAndQueries;
using TheResistanceOnline.Data;
using TheResistanceOnline.Data.Entities;

namespace TheResistanceOnline.Authentications.ExternalIdentities.AuthenticateUserWithReddit;

public class AuthenticateUserWithRedditCommandHandler: ICommandHandler<AuthenticateUserWithRedditCommand, UserId>
{
    #region Fields

    private readonly IDataContext _dataContext;
    private readonly RedditSettings _redditSettings;
    private readonly UserManager<User> _userManager;

    #endregion

    #region Construction

    public AuthenticateUserWithRedditCommandHandler(IDataContext dataContext,
                                                    UserManager<User> userManager,
                                                    IOptions<AppSettings> appSettings)
    {
        _dataContext = dataContext;
        _userManager = userManager;
        _redditSettings = appSettings.Value.AuthServerSettings.RedditSettings;
    }

    #endregion

    #region Public Methods

    public async Task<Result<UserId>> Handle(AuthenticateUserWithRedditCommand command, CancellationToken cancellationToken)
    {
        if (command == null)
        {
            return Result.Failure<UserId>(Error.NullValue);
        }

        if (command.RedditId == null || 
            !string.IsNullOrEmpty(command.RedditId.Value) ||
            command.Audience != _redditSettings.ClientId)
        {
            return Result.Failure<UserId>(ExternalIdentityErrors.MissingIdentifier);
        }

        var redditUser = await _dataContext.Query<IRedditUserByIdDbQuery>()
                                           .WithParams(command.RedditId)
                                           .ExecuteAsync(cancellationToken);
        // todo 
        // continue implementing the authenticateUserWithReddit following the newly implemented results pattern
        // create reddit user in dabasebase (id looks like this [23] = {Claim} id: f08moxny)
        // after this implement the courses implementeation of queries
        // then update all handlers to follow the results pattern
        // dont forget the generic pipeline fluent validation
        // 
    }

    #endregion
}
