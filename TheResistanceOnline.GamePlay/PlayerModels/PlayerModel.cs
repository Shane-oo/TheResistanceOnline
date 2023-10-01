using TheResistanceOnline.GamePlay.Common;
using TheResistanceOnline.GamePlay.GameModels;

namespace TheResistanceOnline.GamePlay.PlayerModels;

public abstract class PlayerModel
{
    #region Properties

    public bool IsBot { get; set; }

    public string Name { get; set; }

    public Team Team { get; set; }

    public bool IsMissionLeader { get; set; }

    #endregion

    #region Construction

    protected PlayerModel(string name)
    {
        Name = name;
    }

    protected PlayerModel(string name, bool isBot)
    {
        Name = name;
        IsBot = isBot;
    }

    #endregion

    #region Public Methods

    public abstract bool Vote();

    // Returns a list of Chosen Players to be on mission
    public abstract List<string> PickTeam(List<string> chosenPlayers = null);

    #endregion
}
