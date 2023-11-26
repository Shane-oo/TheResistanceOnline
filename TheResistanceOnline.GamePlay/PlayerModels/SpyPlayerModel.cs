using TheResistanceOnline.GamePlay.ObserverPattern;

namespace TheResistanceOnline.GamePlay.PlayerModels;

public class SpyPlayerModel: PlayerModel
{
    #region Construction

    public SpyPlayerModel(string name, IGameModelSubject gameModel): base(name, gameModel)
    {
    }

    #endregion

    #region Construction

    protected SpyPlayerModel(string name, IGameModelSubject gameModelSubject, bool isBot): base(name, gameModelSubject, isBot)
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
