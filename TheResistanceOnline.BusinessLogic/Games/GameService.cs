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

        public int GetMissionSize(int missionRound, int playerCount)
        {
            switch(playerCount)
            {
                case FIVE_MAN_GAME:
                    switch(missionRound)
                    {
                        case 1:
                        case 3:
                            return 2;
                        case 2:
                        case 4:
                        case 5:
                            return 3;
                    }

                    break;
                case SIX_MAN_GAME:
                    switch(missionRound)
                    {
                        case 1:
                            return 2;
                        case 2:
                        case 4:
                            return 3;
                        case 3:
                        case 5:
                            return 4;
                    }

                    break;
                case SEVEN_MAN_GAME:
                    switch(missionRound)
                    {
                        case 1:
                            return 2;
                        case 2:
                        case 3:
                            return 3;
                        case 4:
                        case 5:
                            return 4;
                    }

                    break;
                case EIGHT_MAN_GAME:
                case NINE_MAN_GAME:
                case TEN_MAN_GAME:
                    // 8,9,10 man teams are the same
                    switch(missionRound)
                    {
                        case 1:
                            return 3;
                        case 2:
                        case 3:
                            return 4;
                        case 4:
                        case 5:
                            return 5;
                    }

                    break;
            }

            return -1;
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


        public GameDetailsModel SetUpNewGame(GameDetailsModel gameDetails)
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
                //todo put this back after testing mission leader functionality
                /*
                gameDetails.PlayersDetails = shuffledPlayerDetails*/

                gameDetails.PlayersDetails = shuffledPlayerDetails.OrderBy(x => x.IsBot).ToList();
                gameDetails.PlayersDetails.First().IsMissionLeader = true;
            }

            gameDetails.MissionTeam = new List<PlayerDetailsModel>();
            gameDetails.MissionRound = 1;
            gameDetails.MissionSize = GetMissionSize(gameDetails.MissionRound, gameDetails.PlayersDetails!.Count);
            gameDetails.GameStage = GameStageModel.GameStart;
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
