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
        var selectedTeamMember = gameModel.GetPlayerModel(command.PlayerName);


        // New Team Member
        if (gameModel.MissionTeam.All(p => p != selectedTeamMember) && !gameModel.MissionTeamFull())
        {
            player.PickMissionTeamMember(selectedTeamMember);

            await _resistanceHubContext.Clients.Group(command.LobbyId).NewMissionTeamMember(selectedTeamMember.Name);

            if (command.GameModel.MissionTeamFull())
            {
                await _resistanceHubContext.Clients.Client(command.ConnectionId).ShowMissionTeamSubmit(true);
            }
        }
        else
        {
            player.RemoveMissionTeamMember(selectedTeamMember);

            await _resistanceHubContext.Clients.Group(command.LobbyId).RemoveMissionTeamMember(selectedTeamMember.Name);

            await _resistanceHubContext.Clients.Client(command.ConnectionId).ShowMissionTeamSubmit(false);
        }

        return default;
    }

    #endregion
}
