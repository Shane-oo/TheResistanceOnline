namespace TheResistanceOnline.BusinessLogic.Games.Models;

public interface IBotObserver
{
    void GetChoice();

    bool GetVote();

    bool GetMissionChoice();

    List<PlayerDetailsModel> GetMissionProposal();

    void Update(GameDetailsModel gameDetails);

    Guid PlayerId { get; set; }

    void SetPlayerId(Guid playerId);
}
