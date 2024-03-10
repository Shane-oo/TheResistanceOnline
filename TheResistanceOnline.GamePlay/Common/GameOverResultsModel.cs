namespace TheResistanceOnline.GamePlay.Common;

public class GameOverResultsModel
{
    #region Properties

    public Dictionary<string, Team> PlayerNameToTeam { get; }

    public Team Winners { get; }

    #endregion

    #region Construction

    public GameOverResultsModel(Dictionary<string, Team> playerNameToTeam, Team winners)
    {
        PlayerNameToTeam = playerNameToTeam;
        Winners = winners;
    }

    #endregion
}
