using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using TheResistanceOnline.Core;
using TheResistanceOnline.Core.Errors;
using TheResistanceOnline.Core.NewCommandAndQueriesAndResultsPattern;
using TheResistanceOnline.Data;
using TheResistanceOnline.Data.Entities;

namespace TheResistanceOnline.Authentications.ExternalIdentities;

public class AuthenticateUserWithGoogleHandler: ICommandHandler<AuthenticateUserWithGoogleCommand, UserId>
{
    #region Fields

    private readonly IDataContext _context;

    private readonly GoogleSettings _googleSettings;

    private readonly UserManager<User> _userManager;

    #endregion

    #region Construction

    public AuthenticateUserWithGoogleHandler(IDataContext context,
                                             UserManager<User> userManager,
                                             IOptions<AppSettings> appSettings)
    {
        _context = context;
        _userManager = userManager;
        _googleSettings = appSettings.Value.AuthServerSettings.GoogleSettings;
    }

    #endregion

    #region Private Methods

    private async Task<Result<UserId>> CreateUser(AuthenticateUserWithGoogleCommand command)
    {
        var user = GoogleUser.Create(command.GoogleId).User;

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

    public async Task<Result<UserId>> Handle(AuthenticateUserWithGoogleCommand command, CancellationToken cancellationToken)
    {
        if (command == null)
        {
            return Result.Failure<UserId>(Error.NullValue);
        }

        // Validate the audience
        // the aud claim identifies the intended audience of the token, the audience must be client ID of the App
        if (command.Audience != _googleSettings.ClientId)
        {
            return Result.Failure<UserId>(ExternalIdentityErrors.MissingIdentifier);
        }

        var googleUser = await _context.Query<IGoogleUserBySubjectDbQuery>()
                                       .WithParams(command.GoogleId)
                                       .WithNoTracking()
                                       .ExecuteAsync(cancellationToken);

        if (googleUser != null)
        {
            return googleUser.UserId;
        }

        return await CreateUser(command);
    }

    #endregion
}
