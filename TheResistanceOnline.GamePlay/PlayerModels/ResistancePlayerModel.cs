namespace TheResistanceOnline.GamePlay.PlayerModels;

public class ResistancePlayerModel: PlayerModel
{
    #region Construction

    public ResistancePlayerModel(string name): base(name)
    {
    }

    protected ResistancePlayerModel(string name, bool isBot): base(name, isBot)
    {
    }

    #endregion

    #region Public Methods

    public override bool Vote()
    {
        Console.WriteLine("this is the reistance player voting");
        return true;
    }

    #endregion
}
