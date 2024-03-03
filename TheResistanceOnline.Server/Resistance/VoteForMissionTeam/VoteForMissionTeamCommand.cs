using FluentValidation;
using JetBrains.Annotations;
using TheResistanceOnline.Core.Exchange.Requests;
using TheResistanceOnline.GamePlay.Common;
using TheResistanceOnline.GamePlay.GameModels;
using TheResistanceOnline.Server.Common;

namespace TheResistanceOnline.Server.Resistance.VoteForMissionTeam;

public class VoteForMissionTeamCommand: Command, IConnectionModel
{
    #region Properties

    public string CallerPlayerName { get; set; }

    public string ConnectionId { get; set; }

    public GameModel GameModel { get; set; }

    public VotePiece VotePiece { get; set; }

    public string LobbyId { get; set; }

    #endregion
}

[UsedImplicitly]
public class VoteForMissionTeamCommandValidator: AbstractValidator<VoteForMissionTeamCommand>
{
    #region Construction

    public VoteForMissionTeamCommandValidator()
    {
        RuleFor(c => c.GameModel)
            .NotNull();

        RuleFor(c => c.CallerPlayerName)
            .NotEmpty();

        RuleFor(c => c.ConnectionId)
            .NotEmpty();

        RuleFor(c => c.VotePiece)
            .NotNull()
            .IsInEnum();
    }

    #endregion
}
