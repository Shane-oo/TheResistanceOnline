using Microsoft.AspNetCore.SignalR;
using TheResistanceOnline.Core.Errors;
using TheResistanceOnline.Core.Exchange.Responses;
using TheResistanceOnline.Core.NewCommandAndQueriesAndResultsPattern;

namespace TheResistanceOnline.Hubs.Resistance;

public class SelectMissionTeamPlayerHandler: ICommandHandler<SelectMissionTeamPlayerCommand>
{
    #region Fields

    private readonly IHubContext<ResistanceHub, IResistanceHub> _resistanceHubContext;

    #endregion

    #region Construction

    public SelectMissionTeamPlayerHandler(IHubContext<ResistanceHub, IResistanceHub> resistanceHubContext)
    {
        _resistanceHubContext = resistanceHubContext;
    }

    #endregion

    #region Public Methods

    public async Task<Result> Handle(SelectMissionTeamPlayerCommand command, CancellationToken cancellationToken)
    {
        if (command == null)
        {
            return Result.Failure<string>(Error.NullValue);
        }

        var gameModel = command.GameModel;

        var player = gameModel.GetPlayerModel(command.CallerPlayerName);


        // Player selected a new team member
        if (gameModel.MissionTeam.All(p => p != command.SelectedPlayerName) && !gameModel.MissionTeamFull())
        {
            player.PickMissionTeamMember(command.SelectedPlayerName);

            await _resistanceHubContext.Clients.Group(command.LobbyId).NewMissionTeamMember(command.SelectedPlayerName);

            if (command.GameModel.MissionTeamFull())
            {
                await _resistanceHubContext.Clients.Client(command.ConnectionId).ShowMissionTeamSubmit(true);
            }
        }
        else if (gameModel.MissionTeam.Any(p => p == command.SelectedPlayerName))
        {
            // Player wants to remove team member
            player.RemoveMissionTeamMember(command.SelectedPlayerName);

            await _resistanceHubContext.Clients.Group(command.LobbyId).RemoveMissionTeamMember(command.SelectedPlayerName);

            await _resistanceHubContext.Clients.Client(command.ConnectionId).ShowMissionTeamSubmit(false);
        }
        else
        {
            return Result.Failure(new Error("SelectionMissionTeamMember.FullTeam", "Mission Team is Full"));
        }

        return Result.Success();
    }

    #endregion
}
