using TheResistanceOnline.Common.Extensions;
using TheResistanceOnline.GamePlay.BotModels.BotFactories;
using TheResistanceOnline.GamePlay.Common;
using TheResistanceOnline.GamePlay.PlayerModels;

namespace TheResistanceOnline.GamePlay.GameModels;

public class ResistanceClassicGameModel: GameModel
{
    #region Private Methods

    private void AssignTeams(List<string> playerUserNames, int botCount)
    {
        var playerSetupModels = CreatePlayerSetupModels(playerUserNames, botCount);

        IBotFactory botFactory = new ResistanceClassicBotFactory();

        playerSetupModels.Shuffle();
        var resistancePlayers = new List<PlayerSetupModel>();
        var spyPlayers = new List<PlayerSetupModel>();

        var resistanceCount = 0;
        switch(playerSetupModels.Count)
        {
            case 5:
                resistanceCount = 3;
                resistancePlayers = playerSetupModels.Take(resistanceCount).ToList();
                spyPlayers = playerSetupModels.Skip(resistanceCount).ToList();
                break;
            case 6:
                resistanceCount = 4;
                resistancePlayers = playerSetupModels.Take(resistanceCount).ToList();
                spyPlayers = playerSetupModels.Skip(resistanceCount).ToList();
                break;
            case 7:
                resistanceCount = 4;
                resistancePlayers = playerSetupModels.Take(resistanceCount).ToList();
                spyPlayers = playerSetupModels.Skip(resistanceCount).ToList();
                break;
            case 8:
                resistanceCount = 5;
                resistancePlayers = playerSetupModels.Take(resistanceCount).ToList();
                spyPlayers = playerSetupModels.Skip(resistanceCount).ToList();
                break;
            case 9:
                resistanceCount = 6;
                resistancePlayers = playerSetupModels.Take(resistanceCount).ToList();
                spyPlayers = playerSetupModels.Skip(resistanceCount).ToList();
                break;
            case 10:
                resistanceCount = 6;
                resistancePlayers = playerSetupModels.Take(resistanceCount).ToList();
                spyPlayers = playerSetupModels.Skip(resistanceCount).ToList();
                break;
        }

        foreach(var resistancePlayer in resistancePlayers)
        {
            var player = resistancePlayer.IsBot
                             ? botFactory.CreateResistanceBot(resistancePlayer.Name, this)
                             : new ResistancePlayerModel(resistancePlayer.Name);
            player.Team = Team.Resistance;
            Players.Add(player.Name, player);
        }

        foreach(var spyPlayer in spyPlayers)
        {
            var player = spyPlayer.IsBot
                             ? botFactory.CreateSpyBot(spyPlayer.Name, this)
                             : new SpyPlayerModel(spyPlayer.Name);
            player.Team = Team.Spy;
            Players.Add(player.Name, player);
        }
    }

    #endregion

    #region Public Methods

    public override void SetupGame(List<string> playerUserNames, int botCount)
    {
        AssignTeams(playerUserNames, botCount);
        var missionLeader = Players.First();
        UpdateMissionLeader(missionLeader.Key);
        if (missionLeader.Value.IsBot)
        {
            UpdateMissionTeam(missionLeader.Value.PickTeam());
            UpdatePhase(Phase.Vote);
        }
    }

    #endregion
}