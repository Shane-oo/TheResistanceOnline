using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using TheResistanceOnline.Core;
using TheResistanceOnline.Data;
using TheResistanceOnline.Data.Entities;

namespace TheResistanceOnline.Authentications.ExternalIdentities;

public class AuthenticateUserWithGoogleHandler: IRequestHandler<AuthenticateUserWithGoogleCommand, AuthenticationResult<UserId>>
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

    private async Task<AuthenticationResult<UserId>> CreateUser(AuthenticateUserWithGoogleCommand command)
    {
        var user = GoogleUser.Create(command.GoogleId).User;

        var result = await _userManager.CreateAsync(user);

        if (!result.Succeeded)
        {
            var errorDescription = result.Errors.FirstOrDefault()?.Description;
            return Reject(errorDescription);
        }

        await _userManager.AddToRoleAsync(user, Roles.User.ToString());

        return AuthenticationResult<UserId>.Accept(user.Id);
    }

    private static AuthenticationResult<UserId> Reject(string reason)
    {
        return AuthenticationResult<UserId>.Reject(reason);
    }

    #endregion

    #region Public Methods

    public async Task<AuthenticationResult<UserId>> Handle(AuthenticateUserWithGoogleCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        if (command.GoogleId == null || string.IsNullOrEmpty(command.GoogleId.Value))
        {
            return Reject("Missing Google Identifier.");
        }

        // Validate the audience
        // the aud claim identifies the intended audience of the token, the audience must be client ID of the App
        if (command.Audience != _googleSettings.ClientId)
        {
            return Reject("Unauthorized Audience.");
        }

        var googleUser = await _context.Query<IGoogleUserBySubjectDbQuery>()
                                       .WithParams(command.GoogleId)
                                       .WithNoTracking()
                                       .ExecuteAsync(cancellationToken);

        if (googleUser != null)
        {
            return AuthenticationResult<UserId>.Accept(googleUser.UserId);
        }

        return await CreateUser(command);
    }

    #endregion
}
