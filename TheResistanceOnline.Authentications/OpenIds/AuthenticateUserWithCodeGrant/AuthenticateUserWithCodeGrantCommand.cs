using TheResistanceOnline.Core.Requests.Commands;
using TheResistanceOnline.Data.Entities;

namespace TheResistanceOnline.Authentications.OpenIds;

public class AuthenticateUserWithCodeGrantCommand: CommandBase<AuthenticationResult<UserAuthenticationPayload>>
{
    #region Construction

    public AuthenticateUserWithCodeGrantCommand(UserId userId)
    {
        UserId = userId;
    }

    // todo fluent validation

    #endregion
}
