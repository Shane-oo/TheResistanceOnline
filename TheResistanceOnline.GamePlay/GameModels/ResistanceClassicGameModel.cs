using TheResistanceOnline.Common.Extensions;
using TheResistanceOnline.GamePlay.Common;
using TheResistanceOnline.GamePlay.PlayerModels;
using TheResistanceOnline.GamePlay.PlayerModels.BotModels.BotFactories;

namespace TheResistanceOnline.GamePlay.GameModels;

public class ResistanceClassicGameModel: GameModel
{
    #region Private Methods

    private void AssignTeams(List<string> playerUserNames, int botCount)
    {
        var playerSetupModels = CreatePlayerSetupModels(playerUserNames, botCount);

        var botFactory = new ResistanceClassicBotFactory();

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
            PlayerModel player;

            if (resistancePlayer.IsBot)
            {
                var botPlayer = botFactory.CreateResistanceBot(resistancePlayer.Name, this);
                player = botPlayer.PlayerModel;
                player.BotModel = botPlayer.BotModel;
            }
            else
            {
                player = new ResistancePlayerModel(resistancePlayer.Name, this);
            }

            player.Team = Team.Resistance;
            Players.Add(player.Name, player);
        }

        foreach(var spyPlayer in spyPlayers)
        {
            PlayerModel player;

            if (spyPlayer.IsBot)
            {
                var botPlayer = botFactory.CreateSpyBot(spyPlayer.Name, this);
                player = botPlayer.PlayerModel;
                player.BotModel = botPlayer.BotModel;
            }
            else
            {
                player = new SpyPlayerModel(spyPlayer.Name, this);
            }

            player.Team = Team.Spy;
            Players.Add(player.Name, player);
        }
    }

    #endregion

    #region Public Methods

    public override void SetupGame(List<string> playerUserNames, int botCount)
    {
        AssignTeams(playerUserNames, botCount);
        // var missionLeader = Players.First();

        //todo remove
        var missionLeader = Players.First(p => !p.Value.IsBot);

        UpdateMissionLeader(missionLeader.Key);
        if (missionLeader.Value.IsBot)
        {
            for(var i = 0; i < MissionSize; i++)
            {
                missionLeader.Value.PickMissionTeamMember();
            }

            SubmitMissionTeam();
        }
    }

    #endregion
}
