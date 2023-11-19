using FluentValidation;
using TheResistanceOnline.Core.Requests.Commands;
using TheResistanceOnline.GamePlay;

namespace TheResistanceOnline.Hubs.Resistance;

public class StartGameCommand: CommandBase<bool>
{
    #region Properties

    public int Bots { get; set; }

    public bool BotsAllowed { get; set; }

    public GameDetails GameDetails { get; set; }

    public string LobbyId { get; set; }

    public int TotalPlayers { get; set; }

    public GameType Type { get; set; }

    public List<string> UserNames { get; set; }

    #endregion
}

public class StartGameCommandValidator: AbstractValidator<StartGameCommand>
{
    #region Construction

    public StartGameCommandValidator()
    {
        RuleFor(c => c.LobbyId)
            .NotEmpty()
            .NotNull();

        RuleFor(c => c.TotalPlayers)
            .NotNull()
            .LessThanOrEqualTo(10)
            .GreaterThanOrEqualTo(5);
        // No Bots when Bots is not Allowed
        RuleFor(c => c.Bots)
            .Equal(0)
            .When(c => !c.BotsAllowed);

        RuleFor(c => c.UserNames)
            .NotNull()
            .NotEmpty();
    }

    #endregion
}
