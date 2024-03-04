namespace TheResistanceOnline.GamePlay.Common;

public class MissionResultsModel
{
    #region Properties

    public int FailureChoices { get; }

    public int SuccessChoices { get; }
    
    public bool MissionSuccessful { get; }

    #endregion

    #region Construction

    public MissionResultsModel(int successChoices, int failureChoices, bool missionSuccessful)
    {
        SuccessChoices = successChoices;
        FailureChoices = failureChoices;
        MissionSuccessful = missionSuccessful;
    }

    #endregion
}
