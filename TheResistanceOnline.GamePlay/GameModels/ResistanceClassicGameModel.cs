using TheResistanceOnline.Common.Extensions;
using TheResistanceOnline.GamePlay.BotModels.BotFactories;
using TheResistanceOnline.GamePlay.PlayerModels;

namespace TheResistanceOnline.GamePlay.GameModels;

public class ResistanceClassicGameModel: GameModel
{
    #region Public Methods

    public override void AssignTeams(List<string> playerUserNames, int botCount)
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
                             ? botFactory.CreateResistanceBot(resistancePlayer.Name)
                             : new ResistancePlayerModel(resistancePlayer.Name);
            Players.Add(player);
        }

        foreach(var spyPlayer in spyPlayers)
        {
            var player = spyPlayer.IsBot
                             ? botFactory.CreateSpyBot(spyPlayer.Name)
                             : new SpyPlayerModel(spyPlayer.Name);
            Players.Add(player);
        }
    }

    #endregion
}
