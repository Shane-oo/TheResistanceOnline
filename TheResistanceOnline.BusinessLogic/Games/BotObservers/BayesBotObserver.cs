using JetBrains.Annotations;
using TheResistanceOnline.BusinessLogic.Games.Models;

namespace TheResistanceOnline.BusinessLogic.Games.BotObservers;

public class BayesBotObserver: IBotObserver
{
    #region Fields

    private GameDetailsModel _gameDetails = new GameDetailsModel();

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

    #endregion

    #region Public Methods

    public void GetChoice()
    {
        Console.WriteLine("asked for choice");
    }

    public bool GetVote()
    {
        // todo vote on the current mission team
        return true;
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

