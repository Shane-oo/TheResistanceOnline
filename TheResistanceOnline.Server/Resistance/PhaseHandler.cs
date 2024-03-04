using Microsoft.AspNetCore.SignalR;
using TheResistanceOnline.GamePlay.Common;
using TheResistanceOnline.GamePlay.GameModels;

namespace TheResistanceOnline.Server.Resistance;

public static class PhaseHandler
{
    #region Public Methods

    public static async Task HandleNextPhase(IHubContext<ResistanceHub, IResistanceHub> resistanceHubContext, GameModel gameModel, GameDetails gameDetails, string lobbyId)
    {
        switch(gameModel.Phase)
        {
            case Phase.MissionBuild:
                var missionLeaderConnection = gameDetails.Connections.FirstOrDefault(p => p.UserName == gameModel.MissionLeader.Name);
                if (missionLeaderConnection != null)
                {
                    await resistanceHubContext.Clients.Client(missionLeaderConnection.ConnectionId).StartMissionBuildPhase();
                }

                break;
            case Phase.Vote:
                var missionTeam = gameModel.GetMissionTeam().ToList();
                foreach(var teamMember in missionTeam)
                {
                    await resistanceHubContext.Clients.Group(lobbyId).NewMissionTeamMember(teamMember.Name);
                }

                await resistanceHubContext.Clients.Group(lobbyId).VoteForMissionTeam(missionTeam.Select(m => m.Name));

                foreach(var bot in gameModel.Bots)
                {
                    await resistanceHubContext.Clients.Group(lobbyId).PlayerVoted(bot.Name);
                }

                break;
            case Phase.Mission:
                var missionTeamConnections = gameDetails.Connections.Where(c => gameModel.MissionTeam.Exists(p => p.Name == c.UserName));
                foreach(var missionTeamConnection in missionTeamConnections)
                {
                    var team = gameModel.GetPlayerModel(missionTeamConnection.UserName).Team;
                    await resistanceHubContext.Clients.Client(missionTeamConnection.ConnectionId).ShowMissionCards(team == Team.Spy);
                }

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    #endregion
}
