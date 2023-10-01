namespace TheResistanceOnline.GamePlay.Common;

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