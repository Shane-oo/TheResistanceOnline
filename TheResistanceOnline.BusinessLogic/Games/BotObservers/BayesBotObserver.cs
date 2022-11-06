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

    #region Public Methods

    public void GetChoice()
    {
        Console.WriteLine("asked for choice");
    }

    public void Update(GameDetailsModel gameDetails)
    {
        Console.WriteLine("this is the bots playerId", PlayerId);
        _gameDetails = gameDetails;
    }

    #endregion
}
