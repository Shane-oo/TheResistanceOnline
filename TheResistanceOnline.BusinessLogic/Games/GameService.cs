using TheResistanceOnline.BusinessLogic.Games.Models;

namespace TheResistanceOnline.BusinessLogic.Games
{
    public interface IGameService: IGameSubject
    {
        List<IBotObserver> CreateBotObservers(int botCount);
        
        void UpdateGameDetails(GameDetailsModel gameDetails);
    }

    public class GameService: IGameService, IGameObserver
    {
        #region Fields

        private readonly List<IBotObserver> _observers = new List<IBotObserver>();

        #endregion

        #region Public Methods

        // Subject Function
        public void Attach(IBotObserver observer)
        {
            _observers.Add(observer);
        }

        public List<IBotObserver> CreateBotObservers(int botCount)
        {
            for(var i = 0; i < botCount; i++)
            {
                var botObserver = new BayesBotObserver();
                Attach(botObserver);
            }

            return _observers;
        }

        // Subject Function
        public void Detach(IBotObserver observer)
        {
            _observers.Remove(observer);
        }

        public void Dispose()
        {
            foreach(var observer in _observers)
            {
                Detach(observer);
            }
        }

        // Subject Function
        public void Notify(GameDetailsModel gameDetails)
        {
            foreach(var observer in _observers)
            {
                observer.Update(gameDetails);
            }
        }

        // Observer Function
        public void Update(GameDetailsModel gameDetails)
        {
            Console.WriteLine(" Revieved GameDetails from the resistance hub subject", gameDetails);

            UpdateGameDetails(gameDetails);
        }

        // Subject Function
        public void UpdateGameDetails(GameDetailsModel gameDetails)
        {
            Notify(gameDetails);
        }

        #endregion
    }
}
