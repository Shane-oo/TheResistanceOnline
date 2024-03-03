using TheResistanceOnline.GamePlay.ObserverPattern;
using TheResistanceOnline.GamePlay.PlayerModels.BotModels.ResistanceClassicBotModels.ResistanceBots;
using TheResistanceOnline.GamePlay.PlayerModels.BotModels.ResistanceClassicBotModels.SpyBots;

namespace TheResistanceOnline.GamePlay.PlayerModels.BotModels.BotFactories;

public class ResistanceClassicBotFactory: IBotFactory
{
    #region Public Methods

    public PlayerBotModel CreateResistanceBot(string name, IGameModelSubject gameModel)
    {
        // Todo Choose a ResistanceGame Resistance Bot At Random
        var bot = new RandomResistanceBotModel(name, gameModel);
        return new PlayerBotModel
               {
                   BotModel = bot,
                   PlayerModel = bot
               };
    }

    public PlayerBotModel CreateSpyBot(string name, IGameModelSubject gameModel)
    {
        // Todo Choose a ResistanceGame Spy Bot At random
        var bot = new RandomSpyBotModel(name, gameModel);
        return new PlayerBotModel
               {
                   BotModel = bot,
                   PlayerModel = bot
               };
    }

    #endregion
}
