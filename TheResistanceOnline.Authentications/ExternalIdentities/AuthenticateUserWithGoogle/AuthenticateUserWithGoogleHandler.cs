using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using TheResistanceOnline.Common.Extensions;
using TheResistanceOnline.Core;
using TheResistanceOnline.Data;
using TheResistanceOnline.Data.Entities.ExternalIdentitiesEntities;
using TheResistanceOnline.Data.Entities.UserEntities;

namespace TheResistanceOnline.Authentications.ExternalIdentities.AuthenticateUserWithGoogle;

public class AuthenticateUserWithGoogleHandler: IRequestHandler<AuthenticateUserWithGoogleCommand, AuthenticationResult<Guid>>
{
    #region Fields

    private readonly AppSettings _appSettings;
    private readonly IDataContext _context;

    private readonly UserManager<User> _userManager;

    #endregion

    #region Construction

    public AuthenticateUserWithGoogleHandler(IDataContext context,
                                             UserManager<User> userManager,
                                             IOptions<AppSettings> appSettings)
    {
        _context = context;
        _userManager = userManager;
        _appSettings = appSettings.Value;
    }

    #endregion

    #region Private Methods

    private async Task<AuthenticationResult<Guid>> CreateUser(AuthenticateUserWithGoogleCommand command)
    {
        var user = new User
                   {
                       UserName = "User" + Ulid.NewUlid(),
                       GoogleUser = new GoogleUser
                                    {
                                        Subject = command.Subject
                                    },
                       UserSetting = new UserSetting()
                   };

        var result = await _userManager.CreateAsync(user);

        if (!result.Succeeded)
        {
            var errorDescription = result.Errors.FirstOrDefault()?.Description;
            return Reject(errorDescription);
        }

        await _userManager.AddToRoleAsync(user, Roles.User.ToString());

        return AuthenticationResult<Guid>.Accept(user.Id);
    }

    private static AuthenticationResult<Guid> Reject(string reason)
    {
        return AuthenticationResult<Guid>.Reject(reason);
    }

    #endregion

    #region Public Methods

    public async Task<AuthenticationResult<Guid>> Handle(AuthenticateUserWithGoogleCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        if (!command.Subject.HasValue())
        {
            return Reject("Missing Google Identifier.");
        }

        // Validate the audience
        // the aud claim identifies the intended audience of the token, the audience must be client ID of the App

        if (command.Audience != _appSettings.AuthServerSettings.MicrosoftSettings.ClientId)
        {
            return Reject("Unauthorized audience.");
        }

        var googleUser = await _context.Query<IGoogleUserBySubjectDbQuery>()
                                       .WithParams(command.Subject)
                                       .WithNoTracking()
                                       .ExecuteAsync(cancellationToken);

        if (googleUser != null)
        {
            return AuthenticationResult<Guid>.Accept(googleUser.UserId);
        }

        return await CreateUser(command);
    }

    #endregion
}
