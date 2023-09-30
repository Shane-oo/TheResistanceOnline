using TheResistanceOnline.GamePlay.BotModels.ResistanceClassicBotModels.ResistanceBots;
using TheResistanceOnline.GamePlay.BotModels.ResistanceClassicBotModels.SpyBots;
using TheResistanceOnline.GamePlay.PlayerModels;

namespace TheResistanceOnline.GamePlay.BotModels.BotFactories;

public class ResistanceClassicBotFactory: IBotFactory
{
    #region Public Methods

    public ResistancePlayerModel CreateResistanceBot(string name)
    {
        // Choose a ResistanceGame Resistance Bot At Random
        return new DumbResistanceBotModel(name);
    }

    public SpyPlayerModel CreateSpyBot(string name)
    {
        // Choose a ResistanceGame Spy Bot At random
        return new DumbSpyBotModel(name);
    }

    #endregion
}
