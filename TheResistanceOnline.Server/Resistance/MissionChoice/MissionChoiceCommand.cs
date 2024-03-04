using FluentValidation;
using JetBrains.Annotations;
using TheResistanceOnline.Core.Exchange.Requests;
using TheResistanceOnline.Server.Common;

namespace TheResistanceOnline.Server.Resistance.MissionChoice;

public class MissionChoiceCommand: Command, IConnectionModel
{
    #region Properties

    public string CallerPlayerName { get; set; }

    public string ConnectionId { get; set; }

    public GameDetails GameDetails { get; set; }

    public string LobbyId { get; set; }

    public MissionChoicePiece MissionChoice { get; set; }

    #endregion
}

[UsedImplicitly]
public class MissionChoiceCommandValidator: AbstractValidator<MissionChoiceCommand>
{
    #region Construction

    public MissionChoiceCommandValidator()
    {
        RuleFor(c => c.GameDetails)
            .NotNull();

        RuleFor(c => c.CallerPlayerName)
            .NotEmpty();

        RuleFor(c => c.ConnectionId)
            .NotEmpty();

        RuleFor(c => c.MissionChoice)
            .NotNull()
            .IsInEnum();

        RuleFor(c => c.LobbyId)
            .NotEmpty();
    }

    #endregion
}
