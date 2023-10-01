using TheResistanceOnline.GamePlay.GameModels;
using TheResistanceOnline.GamePlay.ObserverPattern;
using TheResistanceOnline.GamePlay.PlayerModels;

namespace TheResistanceOnline.GamePlay.BotModels.BotFactories;

public interface IBotFactory
{
    public ResistancePlayerModel CreateResistanceBot(string name, IGameModelSubject gameModel);

    public SpyPlayerModel CreateSpyBot(string name, IGameModelSubject gameModel);
}
