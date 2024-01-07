using FluentValidation;
using JetBrains.Annotations;
using TheResistanceOnline.Core.Exchange.Requests;
using TheResistanceOnline.GamePlay.GameModels;
using TheResistanceOnline.Hubs.Common;

namespace TheResistanceOnline.Hubs.Resistance;

public class SelectMissionTeamPlayerCommand: Command, IConnectionModel
{
    #region Properties

    public string CallerPlayerName { get; set; }

    public string ConnectionId { get; set; }

    public GameModel GameModel { get; set; }

    public string LobbyId { get; set; }

    public string SelectedPlayerName { get; set; }

    #endregion
}

[UsedImplicitly]
public class SelectMissionTeamPlayerCommandValidator: AbstractValidator<SelectMissionTeamPlayerCommand>
{
    #region Construction

    public SelectMissionTeamPlayerCommandValidator()
    {
        RuleFor(c => c.CallerPlayerName)
            .NotEmpty();

        RuleFor(c => c.SelectedPlayerName)
            .NotEmpty();

        RuleFor(c => c.LobbyId)
            .NotEmpty();

        RuleFor(c => c.GameModel)
            .NotNull();

        RuleFor(c => c.ConnectionId)
            .NotEmpty();
    }

    #endregion
}
