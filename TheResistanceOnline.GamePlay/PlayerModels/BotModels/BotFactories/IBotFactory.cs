using TheResistanceOnline.GamePlay.ObserverPattern;

namespace TheResistanceOnline.GamePlay.PlayerModels.BotModels.BotFactories;

public interface IBotFactory
{
    public PlayerBotModel CreateResistanceBot(string name, IGameModelSubject gameModel);

    public PlayerBotModel CreateSpyBot(string name, IGameModelSubject gameModel);
}
