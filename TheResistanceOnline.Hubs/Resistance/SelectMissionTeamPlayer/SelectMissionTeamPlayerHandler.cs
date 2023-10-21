using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace TheResistanceOnline.Hubs.Resistance.SelectMissionTeamPlayer;

public class SelectMissionTeamPlayerHandler: IRequestHandler<SelectMissionTeamPlayerCommand, Unit>
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

    public async Task<Unit> Handle(SelectMissionTeamPlayerCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
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
        else
        {
            // Player wants to remove team member
            player.RemoveMissionTeamMember(command.SelectedPlayerName);

            await _resistanceHubContext.Clients.Group(command.LobbyId).RemoveMissionTeamMember(command.SelectedPlayerName);

            await _resistanceHubContext.Clients.Client(command.ConnectionId).ShowMissionTeamSubmit(false);
        }

        return default;
    }

    #endregion
}
