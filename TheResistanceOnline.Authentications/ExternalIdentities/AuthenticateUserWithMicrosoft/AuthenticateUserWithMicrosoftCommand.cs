using TheResistanceOnline.Core.Requests.Commands;
using TheResistanceOnline.Data.Entities;

namespace TheResistanceOnline.Authentications.ExternalIdentities;

public class AuthenticateUserWithMicrosoftCommand: CommandBase<AuthenticationResult<UserId>>
{
    #region Properties

    public string Audience { get; set; }

    public MicrosoftId MicrosoftId { get; set; }

    #endregion

    #region Construction

    public AuthenticateUserWithMicrosoftCommand(string audience, MicrosoftId microsoftId)
    {
        Audience = audience;
        MicrosoftId = microsoftId;
    }
    // todo fluent validation
    #endregion
}
