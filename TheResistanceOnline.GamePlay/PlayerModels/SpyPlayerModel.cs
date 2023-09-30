namespace TheResistanceOnline.GamePlay.PlayerModels;

public class SpyPlayerModel: PlayerModel
{
    #region Construction

    public SpyPlayerModel(string name): base(name)
    {
    }

    protected SpyPlayerModel(string name, bool isBot): base(name, isBot)
    {
    }

    #endregion

    #region Public Methods

    public override bool Vote()
    {
        Console.WriteLine("This is the spy player voting");
        return true;
    }

    #endregion
}
