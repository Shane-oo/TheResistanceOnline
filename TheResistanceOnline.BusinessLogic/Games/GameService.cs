using TheResistanceOnline.BusinessLogic.Games.BotObservers;
using TheResistanceOnline.BusinessLogic.Games.Models;

namespace TheResistanceOnline.BusinessLogic.Games
{
    public interface IGameService: IGameSubject
    {
    }

    public class GameService: IGameService, IGameObserver
    {
        #region Constants

        // const for game sizes
        private const int EIGHT_MAN_GAME = 8;
        private const int FIVE_MAN_GAME = 5;
        private const int NINE_MAN_GAME = 9;
        private const int SEVEN_MAN_GAME = 7;
        private const int SIX_MAN_GAME = 6;
        private const int TEN_MAN_GAME = 10;

        #endregion

        #region Fields

        private GameDetailsModel _gameDetails = new();
        private List<IBotObserver> _observers = new();
        private static readonly Random _random = new();

        private readonly List<string> _randomBotNames = new()
                                                        {
                                                            "WALL - E", "R2D2", "K9", "Optimus Prime", "Rosie", "Bender", "C-3PO", "HAL 9000", "Data", "ASIMO", "The Terminator",
                                                            "Micro", "EVA", "RAM", "Sputnik", "Humanoid", "Chip", "Robo", "Robocop", "Alpha", "Beta", "Gamma", "Siri",
                                                            "Raspberry Pie", "AstroBoy", "Chappie", "Ultron", "Omega", "Hydra", "Pixels"
                                                        };

        #endregion

        #region Private Methods

        private static List<PlayerDetailsModel> GetRandomPlayers(int amount, IReadOnlyList<PlayerDetailsModel> playerList)
        {
            var desiredList = new List<PlayerDetailsModel>();
            var count = 0;
            while(count != amount)
            {
                var randomPlayer = playerList[_random.Next(playerList.Count)];

                // check for if randomly got the same person again
                if (desiredList.Any(p => p.PlayerId == randomPlayer.PlayerId)) continue;

                desiredList.Add(randomPlayer);
                count++;
            }

            return desiredList;
        }

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
            _observers = null;
        }

        public string GetRandomBotName()
        {
            return _randomBotNames.MinBy(x => Guid.NewGuid());
        }

        // Subject Function  - notify all bot observers of game details if updated
        public void Notify()
        {
            foreach(var observer in _observers)
            {
                observer.Update(_gameDetails);
            }
        }


        public static GameDetailsModel ShufflePlayerOrderAndAssignTeams(GameDetailsModel gameDetails)
        {
            if (gameDetails.PlayersDetails != null)
            {
                var shuffledPlayerDetails = gameDetails.PlayersDetails.OrderBy(p => _random.Next()).ToList();

                var spyPlayers = new List<PlayerDetailsModel>();
                switch(shuffledPlayerDetails.Count)
                {
                    case FIVE_MAN_GAME:
                    case SIX_MAN_GAME:
                        spyPlayers = GetRandomPlayers(2, shuffledPlayerDetails);
                        break;
                    case SEVEN_MAN_GAME:
                    case EIGHT_MAN_GAME:
                    case NINE_MAN_GAME:
                        spyPlayers = GetRandomPlayers(3, shuffledPlayerDetails);
                        break;
                    case TEN_MAN_GAME:
                        spyPlayers = GetRandomPlayers(4, shuffledPlayerDetails);
                        break;
                }

                // check each player to see if on spy team else assign to resistance team 
                foreach(var player in shuffledPlayerDetails)
                {
                    player.Team = spyPlayers.Any(p => p.PlayerId == player.PlayerId) ? TeamModel.Spy : TeamModel.Resistance;
                }

                // denote first player as mission leader
                shuffledPlayerDetails.First().IsMissionLeader = true;
                gameDetails.PlayersDetails = shuffledPlayerDetails;
            }

            return gameDetails;
        }

        // Observer Function - hub calls this function when game details change
        public void Update(GameDetailsModel gameDetails)
        {
            _gameDetails = gameDetails;
            Notify();
        }

        #endregion
    }
}
