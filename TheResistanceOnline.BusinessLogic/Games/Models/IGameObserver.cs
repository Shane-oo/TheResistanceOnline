namespace TheResistanceOnline.BusinessLogic.Games.Models;

public interface IGameObserver
{
    void Update(GameDetailsModel gameDetails);

    void Dispose();
}
