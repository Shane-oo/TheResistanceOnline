using TheResistanceOnline.BusinessLogic.Games.BotObservers;
using TheResistanceOnline.BusinessLogic.Games.BotObservers.BayesAgent;
using TheResistanceOnline.BusinessLogic.Games.Models;

namespace TheResistanceOnline.BusinessLogic.Games;

public class GameSubjectAndObserver: IGameSubject, IGameObserver
{
    private GameDetailsModel _gameDetails = new();

    private List<IBotObserver> _observers = new();

    private readonly INaiveBayesClassifierService _naiveBayesClassifier;
    public GameSubjectAndObserver(INaiveBayesClassifierService naiveBayesClassifierService)
    {
        _naiveBayesClassifier = naiveBayesClassifierService;
    }
    // Subject Function
    public void Attach(IBotObserver observer)
    {
        _observers.Add(observer);
    }

    // Subject Function
    public void Detach(IBotObserver observer)
    {
        _observers.Remove(observer);
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

    public void Dispose()
    {
        _observers = null;
    }


    //todo
    public List<IBotObserver> CreateBotObservers(int botCount)
    {
        for(var i = 0; i < botCount; i++)
        {
            var botObserver = new BayesBotObserver(_naiveBayesClassifier);

            Attach(botObserver);
        }

        return _observers;
    }
}
