using JetBrains.Annotations;
using TheResistanceOnline.BusinessLogic.Games.Models;

namespace TheResistanceOnline.BusinessLogic.Games.BotObservers;

public class BayesBotObserver: IBotObserver
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
    private static readonly Random _random = new();

    #endregion

    #region Properties

    public Guid PlayerId { get; set; }

    #endregion

    #region Private Methods

    [CanBeNull]
    private PlayerDetailsModel GetMissionLeader(GameDetailsModel gameDetails)
    {
        return gameDetails.PlayersDetails?.FirstOrDefault(p => p.IsMissionLeader);
    }

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

    public void GetChoice()
    {
        Console.WriteLine("asked for choice");
    }

    public List<PlayerDetailsModel> GetMissionProposal()
    {
        var missionSize = GetMissionSize(_gameDetails.MissionRound, _gameDetails.PlayersDetails!.Count);
        return GetRandomPlayers(missionSize, _gameDetails.PlayersDetails);
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

    public bool GetVote()
    {
        // todo vote on the current mission team
        return false;
    }

    public void SetPlayerId(Guid playerId)
    {
        PlayerId = playerId;
    }

    public void Update(GameDetailsModel gameDetails)
    {
        _gameDetails = gameDetails;
        var missionLeader = GetMissionLeader(_gameDetails);
        var botIsMissionLeader = missionLeader?.PlayerId == PlayerId;
        switch(_gameDetails.GameAction)
        {
            case GameActionModel.SubmitMissionPropose:
                // do noting for now i think
                break;
        }


        switch(_gameDetails.GameStage)
        {
            case GameStageModel.VoteResults:
                // do something with VoteResults
                break;
        }
    }

    #endregion
}




