using FluentValidation;
using TheResistanceOnline.Core.NewCommandAndQueries;
using TheResistanceOnline.Data.Entities;
using TheResistanceOnline.Data.Entities.Reddit;

namespace TheResistanceOnline.Authentications.ExternalIdentities.AuthenticateUserWithReddit;

public sealed class AuthenticateUserWithRedditCommand: Command<UserId>
{
    #region Properties

    public RedditId RedditId { get; set; }

    public string Audience { get; set; }

    #endregion

    #region Construction

    public AuthenticateUserWithRedditCommand(UserId userId, Roles userRole): base(userId, userRole)
    {
    }

    #endregion
}

public class AuthenticateUserWithRedditCommandValidator: AbstractValidator<AuthenticateUserWithRedditCommand>
{
    #region Construction

    public AuthenticateUserWithRedditCommandValidator()
    {
        RuleFor(c => c.RedditId).NotNull();
        RuleFor(c => c.RedditId.Value).NotEmpty();
    }

    #endregion
}
