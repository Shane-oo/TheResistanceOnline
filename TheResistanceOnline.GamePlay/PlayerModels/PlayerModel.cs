namespace TheResistanceOnline.GamePlay.PlayerModels;

public abstract class PlayerModel
{
    #region Properties

    public string Name { get; set; }

    public bool IsBot { get; set; }

    protected PlayerModel(string name)
    {
        Name = name;
    }

    protected PlayerModel(string name, bool isBot)
    {
        Name = name;
        IsBot = isBot;
    }

    public abstract bool Vote();

    #endregion
}
