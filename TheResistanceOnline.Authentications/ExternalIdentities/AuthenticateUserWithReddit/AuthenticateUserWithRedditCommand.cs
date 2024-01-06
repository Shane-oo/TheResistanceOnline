using FluentValidation;
using JetBrains.Annotations;
using TheResistanceOnline.Core.Exchange.Requests;
using TheResistanceOnline.Data.Entities;

namespace TheResistanceOnline.Authentications.ExternalIdentities;

public sealed class AuthenticateUserWithRedditCommand: Command<UserId>
{
    #region Properties

    public string Audience { get; }

    public RedditId RedditId { get; }

    #endregion

    #region Construction

    public AuthenticateUserWithRedditCommand(RedditId redditId, string audience)
    {
        RedditId = redditId;
        Audience = audience;
    }

    #endregion
}

[UsedImplicitly]
public class AuthenticateUserWithRedditCommandValidator: AbstractValidator<AuthenticateUserWithRedditCommand>
{
    #region Construction

    public AuthenticateUserWithRedditCommandValidator()
    {
        RuleFor(c => c.RedditId).NotNull();
        RuleFor(c => c.RedditId.Value).NotEmpty();
        RuleFor(c => c.Audience).NotEmpty();
    }

    #endregion
}
