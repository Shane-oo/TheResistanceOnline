namespace TheResistanceOnline.BusinessLogic.Games.Models;

public interface IGamePlayingBotObserver
{
    void GetChoice();

    bool GetMissionChoice();

    List<PlayerDetailsModel> GetMissionProposal();

    string GetName();

    TeamModel GetTeam();

    bool GetVote();

    void SetPlayerId(Guid playerId);

    void SetTeam(TeamModel team);

    Guid PlayerId { get; set; }

    string GetSpyPredictions();
}
