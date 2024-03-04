using Microsoft.AspNetCore.SignalR;
using TheResistanceOnline.Core.Errors;
using TheResistanceOnline.Core.Exchange.Requests;
using TheResistanceOnline.Core.Exchange.Responses;

namespace TheResistanceOnline.Server.Resistance.MissionChoice;

public class MissionChoiceHandler: ICommandHandler<MissionChoiceCommand>
{
    #region Fields

    private readonly IHubContext<ResistanceHub, IResistanceHub> _resistanceHubContext;

    #endregion

    #region Construction

    public MissionChoiceHandler(IHubContext<ResistanceHub, IResistanceHub> resistanceHubContext)
    {
        _resistanceHubContext = resistanceHubContext;
    }

    #endregion

    #region Public Methods

    public async Task<Result> Handle(MissionChoiceCommand command, CancellationToken cancellationToken)
    {
        if (command is null) return Result.Failure(Error.NullValue);

        var gameModel = command.GameDetails.GameModel;

        var player = gameModel.GetPlayerModel(command.CallerPlayerName);

        var choseSuccess = command.MissionChoice == MissionChoicePiece.MissionSuccessPiece;
        player.SubmitMissionOutcome(choseSuccess);

        await _resistanceHubContext.Clients.Client(command.ConnectionId).RemoveMissionChoices();

        var missionOverResult = gameModel.GetMissionResults();
        if (missionOverResult.IsSuccess)
        {
            await _resistanceHubContext.Clients.Group(command.LobbyId).ShowMissionResults(missionOverResult.Value);
            // wait for 10 seconds before removing missionResults
            Thread.Sleep(10000);
        }

        await PhaseHandler.HandleNextPhase(_resistanceHubContext, gameModel, command.GameDetails, command.LobbyId);

        return Result.Success();
    }

    #endregion
}
