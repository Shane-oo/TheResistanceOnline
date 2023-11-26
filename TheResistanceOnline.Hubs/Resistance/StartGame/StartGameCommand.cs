using FluentValidation;
using JetBrains.Annotations;
using TheResistanceOnline.Core.NewCommandAndQueriesAndResultsPattern;
using TheResistanceOnline.GamePlay;
using TheResistanceOnline.Hubs.Common;

namespace TheResistanceOnline.Hubs.Resistance;

public class StartGameCommand: Command<bool>, IConnectionModel
{
    #region Properties

    public int Bots { get; set; }

    public bool BotsAllowed { get; set; }

    public string ConnectionId { get; set; }

    public GameDetails GameDetails { get; set; }

    public string LobbyId { get; set; }

    public int TotalPlayers { get; set; }

    public GameType Type { get; set; }

    public List<string> UserNames { get; set; }

    #endregion
}

[UsedImplicitly]
public class StartGameCommandValidator: AbstractValidator<StartGameCommand>
{
    #region Construction

    public StartGameCommandValidator()
    {
        RuleFor(c => c.LobbyId)
            .NotEmpty();

        RuleFor(c => c.TotalPlayers)
            .NotNull()
            .LessThanOrEqualTo(10)
            .GreaterThanOrEqualTo(5);
        // No Bots when Bots is not Allowed
        RuleFor(c => c.Bots)
            .Equal(0)
            .When(c => !c.BotsAllowed);

        RuleFor(c => c.UserNames)
            .NotEmpty();

        RuleFor(c => c.ConnectionId)
            .NotEmpty();
    }

    #endregion
}
