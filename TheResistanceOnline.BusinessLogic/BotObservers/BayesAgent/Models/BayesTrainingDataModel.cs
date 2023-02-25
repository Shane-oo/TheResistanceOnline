namespace TheResistanceOnline.BusinessLogic.BotObservers.BayesAgent.Models;

public class BayesTrainingDataModel
{
    #region Properties

    public int ResistanceRows { get; set; }

    public int SpyRows { get; set; }


    public Dictionary<int, List<StatsModel>> Statistics { get; set; }

    #endregion

    #region Public Methods

    public int TotalRows()
    {
        return SpyRows + ResistanceRows;
    }

    #endregion
}
