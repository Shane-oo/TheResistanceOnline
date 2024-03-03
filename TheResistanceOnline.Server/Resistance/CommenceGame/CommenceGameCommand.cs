using FluentValidation;
using JetBrains.Annotations;
using TheResistanceOnline.Core.Exchange.Requests;

namespace TheResistanceOnline.Server.Resistance;

public class CommenceGameCommand: Command

{
    #region Properties

    public GameDetails GameDetails { get; set; }

    public string LobbyId { get; set; }

    #endregion
}

[UsedImplicitly]
public class CommenceGameCommandValidator: AbstractValidator<CommenceGameCommand>
{
    #region Construction

    public CommenceGameCommandValidator()
    {
        RuleFor(c => c.LobbyId)
            .NotEmpty();
    }

    #endregion
}
