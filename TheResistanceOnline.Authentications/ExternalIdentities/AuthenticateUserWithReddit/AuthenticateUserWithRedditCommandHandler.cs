using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using TheResistanceOnline.Core;
using TheResistanceOnline.Core.Errors;
using TheResistanceOnline.Core.Exchange.Requests;
using TheResistanceOnline.Core.Exchange.Responses;
using TheResistanceOnline.Core.NewCommandAndQueriesAndResultsPattern;
using TheResistanceOnline.Data;
using TheResistanceOnline.Data.Entities;
using TheResistanceOnline.Data.Entities;

namespace TheResistanceOnline.Authentications.ExternalIdentities;

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

    #region Private Methods

    private async Task<Result<UserId>> CreateUser(AuthenticateUserWithRedditCommand command, CancellationToken cancellationToken)
    {
        var user = RedditUser.Create(command.RedditId).User;

        var result = await _userManager.CreateAsync(user);

        if (!result.Succeeded)
        {
            var identityError = result.Errors.FirstOrDefault();
            return Result.Failure<UserId>(identityError != null ? new Error(identityError.Code, identityError.Description) : Error.Unknown);
        }

        await _userManager.AddToRoleAsync(user, Roles.User.ToString());

        return user.Id;
    }

    #endregion

    #region Public Methods

    public async Task<Result<UserId>> Handle(AuthenticateUserWithRedditCommand command, CancellationToken cancellationToken)
    {
        if (command == null)
        {
            return Result.Failure<UserId>(Error.NullValue);
        }

        if (command.Audience != _redditSettings.ClientId)
        {
            return Result.Failure<UserId>(ExternalIdentityErrors.MissingIdentifier);
        }

        var redditUser = await _dataContext.Query<IRedditUserByIdDbQuery>()
                                           .WithParams(command.RedditId)
                                           .WithNoTracking()
                                           .ExecuteAsync(cancellationToken);
        if (redditUser != null)
        {
            return redditUser.UserId;
        }

        return await CreateUser(command, cancellationToken);
    }

    #endregion
}
