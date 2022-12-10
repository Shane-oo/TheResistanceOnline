namespace TheResistanceOnline.BusinessLogic.Games.Models;

public interface IBotObserver
{
    void GetChoice();

    bool GetVote();

    void Update(GameDetailsModel gameDetails);

    Guid PlayerId { get; set; }

    void SetPlayerId(Guid playerId);
}
