using TheResistanceOnline.GamePlay.ObserverPattern;

namespace TheResistanceOnline.GamePlay.PlayerModels.BotModels.BotFactories;

public interface IBotFactory
{
    public ResistancePlayerModel CreateResistanceBot(string name, IGameModelSubject gameModel);

    public SpyPlayerModel CreateSpyBot(string name, IGameModelSubject gameModel);
}
