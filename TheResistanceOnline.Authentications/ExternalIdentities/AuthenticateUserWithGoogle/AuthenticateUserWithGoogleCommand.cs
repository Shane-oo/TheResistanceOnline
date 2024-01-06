using FluentValidation;
using JetBrains.Annotations;
using TheResistanceOnline.Core.Exchange.Requests;
using TheResistanceOnline.Core.NewCommandAndQueriesAndResultsPattern;
using TheResistanceOnline.Data.Entities;

namespace TheResistanceOnline.Authentications.ExternalIdentities;

public class AuthenticateUserWithGoogleCommand: Command<UserId>
{
    #region Properties

    public string Audience { get; }

    public GoogleId GoogleId { get; }

    #endregion

    #region Construction

    public AuthenticateUserWithGoogleCommand(string audience, GoogleId googleId)
    {
        Audience = audience;
        GoogleId = googleId;
    }

    #endregion
}

[UsedImplicitly]
public class AuthenticateUserWithGoogleCommandValidator: AbstractValidator<AuthenticateUserWithGoogleCommand>
{
    #region Construction

    public AuthenticateUserWithGoogleCommandValidator()
    {
        RuleFor(c => c.GoogleId).NotNull();
        RuleFor(c => c.GoogleId.Value).NotEmpty();
        RuleFor(c => c.Audience).NotEmpty();
    }

    #endregion
}
