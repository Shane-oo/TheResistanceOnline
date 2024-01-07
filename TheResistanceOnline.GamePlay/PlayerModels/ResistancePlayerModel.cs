using TheResistanceOnline.GamePlay.ObserverPattern;

namespace TheResistanceOnline.GamePlay.PlayerModels;

public class ResistancePlayerModel: PlayerModel
{
    #region Construction

    public ResistancePlayerModel(string name, IGameModelSubject gameModel): base(name, gameModel)
    {
    }

    #endregion

    #region Construction

    protected ResistancePlayerModel(string name, IGameModelSubject gameModelSubject, bool isBot): base(name, gameModelSubject, isBot)
    {
    }

    #endregion
}
