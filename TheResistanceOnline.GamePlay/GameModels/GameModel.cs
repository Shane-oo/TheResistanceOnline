using TheResistanceOnline.GamePlay.PlayerModels;

namespace TheResistanceOnline.GamePlay.GameModels;

public abstract class GameModel
{
    #region Properties

    public int TotalPlayers => Players.Count;

    public List<PlayerModel> Players { get; set; } = new();

    #endregion

    #region Private Methods

    protected static List<PlayerSetupModel> CreatePlayerSetupModels(List<string> userNames, int botCount)
    {
        var playerSetupModels = new List<PlayerSetupModel>();
        foreach(var playerUserName in userNames)
        {
            playerSetupModels.Add(new PlayerSetupModel(playerUserName, false));
        }

        for(var i = 0; i < botCount; i++)
        {
            playerSetupModels.Add(new PlayerSetupModel($"Bot-{i}", true));
        }

        return playerSetupModels;
    }

    #endregion

    #region Public Methods

    public abstract void AssignTeams(List<string> playerUserNames, int botCount);

    #endregion
}

public class PlayerSetupModel
{
    #region Properties

    public bool IsBot { get; set; }

    public string Name { get; set; }

    #endregion

    #region Construction

    public PlayerSetupModel(string name, bool isBot)
    {
        IsBot = isBot;
        Name = name;
    }

    #endregion
}
