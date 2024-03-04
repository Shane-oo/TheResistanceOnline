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


    public override void SubmitMissionOutcome(bool decision)
    {
        // Resistance can only vote yes
        base.SubmitMissionOutcome(true);
    }

    #endregion
}
