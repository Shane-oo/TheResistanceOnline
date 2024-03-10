using Microsoft.AspNetCore.SignalR;
using TheResistanceOnline.GamePlay.Common;
using TheResistanceOnline.GamePlay.GameModels;

namespace TheResistanceOnline.Server.Resistance;

public static class PhaseHandler
{
    #region Public Methods

    public static async Task HandleNextPhase(IHubContext<ResistanceHub, IResistanceHub> resistanceHubContext, GameModel gameModel, GameDetails gameDetails, string lobbyId)
    {
        await resistanceHubContext.Clients.Group(lobbyId).SetMissionLeader(gameModel.MissionLeader.Name);
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
                    await resistanceHubContext.Clients.Client(missionTeamConnection.ConnectionId).ShowMissionChoices(team == Team.Spy);
                }

                break;
            case Phase.GameOver:
                var playerNameToTeam = gameDetails.GameModel.Players.ToDictionary(player => player.Key, player => player.Value.Team);
                await resistanceHubContext.Clients.Group(lobbyId).ShowGameOver(new GameOverResultsModel(playerNameToTeam, gameDetails.GameModel.Winner));
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    #endregion
}
