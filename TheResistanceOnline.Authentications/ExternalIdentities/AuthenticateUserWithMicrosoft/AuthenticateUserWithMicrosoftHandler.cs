using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using TheResistanceOnline.Common.Extensions;
using TheResistanceOnline.Core;
using TheResistanceOnline.Data;
using TheResistanceOnline.Data.Entities.ExternalIdentitiesEntities;
using TheResistanceOnline.Data.Entities.UserEntities;

namespace TheResistanceOnline.Authentications.ExternalIdentities;

public class AuthenticateUserWithMicrosoftHandler: IRequestHandler<AuthenticateUserWithMicrosoftCommand, AuthenticationResult<Guid>>
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

    private async Task<AuthenticationResult<Guid>> CreateUser(AuthenticateUserWithMicrosoftCommand command)
    {
        var user = new User
                   {
                       UserName = "User" + Ulid.NewUlid(),
                       MicrosoftUser = new MicrosoftUser
                                       {
                                           ObjectId = command.ObjectId
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

    public async Task<AuthenticationResult<Guid>> Handle(AuthenticateUserWithMicrosoftCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (!command.ObjectId.HasValue())
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
                                              .WithParams(command.ObjectId)
                                              .WithNoTracking()
                                              .ExecuteAsync(cancellationToken);

        if (microsoftUser != null)
        {
            return AuthenticationResult<Guid>.Accept(microsoftUser.UserId);
        }

        return await CreateUser(command);
    }

    #endregion
}
