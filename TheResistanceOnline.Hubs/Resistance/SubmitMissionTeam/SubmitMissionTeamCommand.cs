using FluentValidation;
using JetBrains.Annotations;
using TheResistanceOnline.Core.Exchange.Requests;
using TheResistanceOnline.GamePlay.GameModels;
using TheResistanceOnline.Hubs.Common;

namespace TheResistanceOnline.Hubs.Resistance;

public class SubmitMissionTeamCommand: Command
{
    public string LobbyId { get; }

    public GameModel GameModel { get; }

    public SubmitMissionTeamCommand(string lobbyId, GameModel gameModel)
    {
        LobbyId = lobbyId;
        GameModel = gameModel;
    }
}

[UsedImplicitly]
public class SubmitMissionTeamCommandValidator: AbstractValidator<SubmitMissionTeamCommand>
{
    public SubmitMissionTeamCommandValidator()
    {
        RuleFor(c => c.LobbyId).NotEmpty();
        RuleFor(c => c.GameModel)
            .NotNull();
    }
}
