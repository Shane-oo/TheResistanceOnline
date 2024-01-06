using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using TheResistanceOnline.Core;
using TheResistanceOnline.Core.Errors;
using TheResistanceOnline.Core.Exchange.Requests;
using TheResistanceOnline.Core.Exchange.Responses;
using TheResistanceOnline.Core.NewCommandAndQueriesAndResultsPattern;
using TheResistanceOnline.Data;
using TheResistanceOnline.Data.Entities;

namespace TheResistanceOnline.Authentications.ExternalIdentities;

public class AuthenticateUserWithMicrosoftHandler: ICommandHandler<AuthenticateUserWithMicrosoftCommand, UserId>
{
    #region Fields

    private readonly IDataContext _dataContext;

    private readonly MicrosoftSettings _microsoftSettings;
    private readonly UserManager<User> _userManager;

    #endregion

    #region Construction

    public AuthenticateUserWithMicrosoftHandler(IDataContext dataContext,
                                                UserManager<User> userManager,
                                                IOptions<AppSettings> appSettings)
    {
        _dataContext = dataContext;
        _userManager = userManager;
        _microsoftSettings = appSettings.Value.AuthServerSettings.MicrosoftSettings;
    }

    #endregion

    #region Private Methods

    private async Task<Result<UserId>> CreateUser(AuthenticateUserWithMicrosoftCommand command, CancellationToken cancellationToken)
    {
        var user = MicrosoftUser.Create(command.MicrosoftId).User;

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

    public async Task<Result<UserId>> Handle(AuthenticateUserWithMicrosoftCommand command, CancellationToken cancellationToken)
    {
        if (command == null)
        {
            return Result.Failure<UserId>(Error.NullValue);
        }

        // Validate the audience
        // the aud claim identifies the intended audience of the token, the audience must be client ID of the App
        if (command.Audience != _microsoftSettings.ClientId)
        {
            return Result.Failure<UserId>(ExternalIdentityErrors.MissingIdentifier);
        }

        var microsoftUser = await _dataContext.Query<IMicrosoftUserByObjectIdDbQuery>()
                                              .WithParams(command.MicrosoftId)
                                              .WithNoTracking()
                                              .ExecuteAsync(cancellationToken);

        if (microsoftUser != null)
        {
            return microsoftUser.UserId;
        }

        return await CreateUser(command, cancellationToken);
    }

    #endregion
}
