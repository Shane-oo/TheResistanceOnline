using TheResistanceOnline.BusinessLogic.Games.Models;

namespace TheResistanceOnline.BusinessLogic.Games;

public class BayesBotObserver:IBotObserver
{
    public void Update(GameDetailsModel gameDetails)
    {
        Console.WriteLine("GameDetails have been updated",gameDetails);
    }
}
