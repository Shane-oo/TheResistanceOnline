using TheResistanceOnline.Core.Commands;

namespace TheResistanceOnline.Authentications.OpenIds.AuthenticateUserWithCodeGrant;

public class AuthenticateUserWithCodeGrantCommand: CommandBase<AuthenticationResult<UserAuthenticationPayload>>
{
    #region Construction

    public AuthenticateUserWithCodeGrantCommand(Guid userId)
    {
        UserId = userId;
    }

    #endregion
}
