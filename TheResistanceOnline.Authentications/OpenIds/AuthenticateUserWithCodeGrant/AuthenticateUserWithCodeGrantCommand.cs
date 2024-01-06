using FluentValidation;
using JetBrains.Annotations;
using TheResistanceOnline.Core.Exchange.Requests;
using TheResistanceOnline.Data.Entities;

namespace TheResistanceOnline.Authentications.OpenIds;

public class AuthenticateUserWithCodeGrantCommand: Command<UserAuthenticationPayload>
{
    #region Construction

    public AuthenticateUserWithCodeGrantCommand(UserId userId): base(userId)
    {
    }

    #endregion
}

[UsedImplicitly]
public class AuthenticateUserWithCodeGrantCommandValidator: AbstractValidator<AuthenticateUserWithCodeGrantCommand>
{
    #region Construction

    public AuthenticateUserWithCodeGrantCommandValidator()
    {
        RuleFor(c => c.UserId).NotNull();
        RuleFor(c => c.UserId.Value).NotEmpty();
    }

    #endregion
}
