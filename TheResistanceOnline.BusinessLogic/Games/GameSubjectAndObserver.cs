using JetBrains.Annotations;
using TheResistanceOnline.BusinessLogic.Games.BotObservers.BayesAgent;
using TheResistanceOnline.BusinessLogic.Games.BotObservers.SpectatorAgent;
using TheResistanceOnline.BusinessLogic.Games.Models;

namespace TheResistanceOnline.BusinessLogic.Games;

public class GameSubjectAndObserver: IGameSubject, IGameObserver
{
    #region Fields

    private GameDetailsModel _gameDetails = new();

    private readonly INaiveBayesClassifierService _naiveBayesClassifier;

    private List<IBotObserver> _observers = new();

    #endregion

    #region Construction

    public GameSubjectAndObserver([CanBeNull] INaiveBayesClassifierService naiveBayesClassifierService)
    {
        _naiveBayesClassifier = naiveBayesClassifierService;
    }

    #endregion

    #region Public Methods

    // Subject Function
    public void Attach(IBotObserver observer)
    {
        _observers.Add(observer);
    }


    public List<IGamePlayingBotObserver> CreateGamePlayingBotObservers(int botCount)
    {
        var gamePlayingBotObservers = new List<IGamePlayingBotObserver>();
        for(var i = 0; i < botCount; i++)
        {
            var botObserver = new BayesBotObserver(_naiveBayesClassifier);

            Attach(botObserver);
            gamePlayingBotObservers.Add(botObserver);
        }

        return gamePlayingBotObservers;
    }

    public ISpectatorBotObserver CreatePlayerValuesSpectatorBotObserver()
    {
        var spectatorBotObserver = new PlayerValuesSpectatorBotObserver();
        Attach(spectatorBotObserver);
        return spectatorBotObserver;
    }

    // Subject Function
    public void Detach(IBotObserver observer)
    {
        _observers.Remove(observer);
    }

    public void Dispose()
    {
        _observers = null;
    }

    // Subject Function  - notify all bot observers of game details if updated
    public void Notify()
    {
        foreach(var observer in _observers)
        {
            observer.Update(_gameDetails);
        }
    }

    // Observer Function - hub calls this function when game details change
    public void Update(GameDetailsModel gameDetails)
    {
        _gameDetails = gameDetails;
        Notify();
    }

    #endregion
}
