using TheResistanceOnline.Core.Requests.Commands;

namespace TheResistanceOnline.Authentications.ExternalIdentities.AuthenticateUserWithGoogle;

public class AuthenticateUserWithGoogleCommand: CommandBase<AuthenticationResult<Guid>>
{
    #region Properties

    public string Audience { get; set; }

    public string Subject { get; set; }

    #endregion

    #region Construction

    public AuthenticateUserWithGoogleCommand(string audience, string subject)
    {
        Audience = audience;
        Subject = subject;
    }

    #endregion
}
