using TheResistanceOnline.Core.Requests.Commands;
using TheResistanceOnline.Data.Entities;

namespace TheResistanceOnline.Authentications.ExternalIdentities;

public class AuthenticateUserWithGoogleCommand: CommandBase<AuthenticationResult<UserId>>
{
    #region Properties

    public string Audience { get; set; }

    public GoogleId GoogleId { get; set; }

    #endregion

    #region Construction

    public AuthenticateUserWithGoogleCommand(string audience, GoogleId googleId)
    {
        Audience = audience;
        GoogleId = googleId;
    }

    // todo fluent validation
    #endregion
}
