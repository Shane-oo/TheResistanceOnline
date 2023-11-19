using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using TheResistanceOnline.Common.Extensions;
using TheResistanceOnline.Core;
using TheResistanceOnline.Data;
using TheResistanceOnline.Data.Entities;

namespace TheResistanceOnline.Authentications.ExternalIdentities;

public class AuthenticateUserWithMicrosoftHandler: IRequestHandler<AuthenticateUserWithMicrosoftCommand, AuthenticationResult<UserId>>
{
    #region Fields

    private readonly AppSettings _appSettings;

    private readonly IDataContext _dataContext;
    private readonly UserManager<User> _userManager;

    #endregion

    #region Construction

    public AuthenticateUserWithMicrosoftHandler(IDataContext dataContext,
                                                UserManager<User> userManager,
                                                IOptions<AppSettings> appSettings)
    {
        _dataContext = dataContext;
        _userManager = userManager;
        _appSettings = appSettings.Value;
    }

    #endregion

    #region Private Methods

    private async Task<AuthenticationResult<UserId>> CreateUser(AuthenticateUserWithMicrosoftCommand command, CancellationToken cancellationToken)
    {
        var user = MicrosoftUser.Create(command.MicrosoftId).User;

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

    public async Task<AuthenticationResult<UserId>> Handle(AuthenticateUserWithMicrosoftCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (command.MicrosoftId == null || !command.MicrosoftId.Value.HasValue())
        {
            return Reject("Missing Microsoft Identifier.");
        }

        // Validate the audience
        // the aud claim identifies the intended audience of the token, the audience must be client ID of the App

        if (command.Audience != _appSettings.AuthServerSettings.MicrosoftSettings.ClientId)
        {
            return Reject("Unauthorized audience.");
        }

        var microsoftUser = await _dataContext.Query<IMicrosoftUserByObjectIdDbQuery>()
                                              .WithParams(command.MicrosoftId)
                                              .WithNoTracking()
                                              .ExecuteAsync(cancellationToken);

        if (microsoftUser != null)
        {
            return AuthenticationResult<UserId>.Accept(microsoftUser.UserId);
        }

        return await CreateUser(command, cancellationToken);
    }

    #endregion
}
