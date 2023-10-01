using TheResistanceOnline.GamePlay.BotModels.ResistanceClassicBotModels.ResistanceBots;
using TheResistanceOnline.GamePlay.BotModels.ResistanceClassicBotModels.SpyBots;
using TheResistanceOnline.GamePlay.GameModels;
using TheResistanceOnline.GamePlay.ObserverPattern;
using TheResistanceOnline.GamePlay.PlayerModels;

namespace TheResistanceOnline.GamePlay.BotModels.BotFactories;

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
