namespace TheResistanceOnline.GamePlay.Common;

public class MissionResultsModel
{
    #region Properties

    public int SabotageChoices { get; }

    public int SuccessChoices { get; }
    
    public bool MissionSuccessful { get; }

    #endregion

    #region Construction

    public MissionResultsModel(int successChoices, int sabotageChoices, bool missionSuccessful)
    {
        SuccessChoices = successChoices;
        SabotageChoices = sabotageChoices;
        MissionSuccessful = missionSuccessful;
    }

    #endregion
}
