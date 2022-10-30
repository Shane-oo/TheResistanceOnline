using TheResistanceOnline.BusinessLogic.Games.Models;

namespace TheResistanceOnline.BusinessLogic.Games.BotObservers;

public class BayesBotObserver:IBotObserver
{
    public void Update(GameDetailsModel gameDetails)
    {
        Console.WriteLine("GameDetails have been updated",gameDetails);
    }

    public void GetChoice()
    {
        Console.WriteLine("asked for choice");
    }
}
