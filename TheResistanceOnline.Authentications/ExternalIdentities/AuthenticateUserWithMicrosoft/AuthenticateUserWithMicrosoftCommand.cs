using TheResistanceOnline.Core.Commands;

namespace TheResistanceOnline.Authentications.ExternalIdentities.AuthenticateUserWithMicrosoft;

public class AuthenticateUserWithMicrosoftCommand: CommandBase<AuthenticationResult<Guid>>
{
    #region Properties

    public string Audience { get; set; }

    public Guid ObjectId { get; set; }

    #endregion

    #region Construction

    public AuthenticateUserWithMicrosoftCommand(string audience, Guid objectId)
    {
        Audience = audience;
        ObjectId = objectId;
    }

    #endregion
}
