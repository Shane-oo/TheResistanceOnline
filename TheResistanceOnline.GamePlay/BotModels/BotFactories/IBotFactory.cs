using TheResistanceOnline.GamePlay.PlayerModels;

namespace TheResistanceOnline.GamePlay.BotModels.BotFactories;

public interface IBotFactory
{
    public ResistancePlayerModel CreateResistanceBot(string name);

    public SpyPlayerModel CreateSpyBot(string name);
}
