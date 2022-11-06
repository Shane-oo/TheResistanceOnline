namespace TheResistanceOnline.BusinessLogic.Games.Models;

public interface IBotObserver
{
    void GetChoice();

    void Update(GameDetailsModel gameDetails);

    Guid PlayerId { get; set; }
}
