using TheResistanceOnline.GamePlay.ObserverPattern;
using TheResistanceOnline.GamePlay.PlayerModels.BotModels.ResistanceClassicBotModels.ResistanceBots;
using TheResistanceOnline.GamePlay.PlayerModels.BotModels.ResistanceClassicBotModels.SpyBots;

namespace TheResistanceOnline.GamePlay.PlayerModels.BotModels.BotFactories;

public class ResistanceClassicBotFactory: IBotFactory
{
    #region Public Methods

    public ResistancePlayerModel CreateResistanceBot(string name, IGameModelSubject gameModel)
    {
        // Choose a ResistanceGame Resistance Bot At Random
        return new RandomResistanceBotModel(name, gameModel);
    }

    public SpyPlayerModel CreateSpyBot(string name, IGameModelSubject gameModel)
    {
        // Choose a ResistanceGame Spy Bot At random
        return new RandomSpyBotModel(name, gameModel);
    }

    #endregion
}
