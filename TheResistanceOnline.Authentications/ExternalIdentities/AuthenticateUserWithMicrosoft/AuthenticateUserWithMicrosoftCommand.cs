using FluentValidation;
using JetBrains.Annotations;
using TheResistanceOnline.Core.Exchange.Requests;
using TheResistanceOnline.Data.Entities;

namespace TheResistanceOnline.Authentications.ExternalIdentities;

public class AuthenticateUserWithMicrosoftCommand: Command<UserId>
{
    #region Properties

    public string Audience { get; }

    public MicrosoftId MicrosoftId { get; }

    #endregion

    #region Construction

    public AuthenticateUserWithMicrosoftCommand(string audience, MicrosoftId microsoftId)
    {
        Audience = audience;
        MicrosoftId = microsoftId;
    }

    #endregion
}

[UsedImplicitly]
public class AuthenticateUserWithMicrosoftCommandValidator: AbstractValidator<AuthenticateUserWithMicrosoftCommand>
{
    #region Construction

    public AuthenticateUserWithMicrosoftCommandValidator()
    {
        RuleFor(c => c.MicrosoftId).NotNull();
        RuleFor(c => c.MicrosoftId.Value).NotEmpty();
        RuleFor(c => c.Audience).NotEmpty();
    }

    #endregion
}
