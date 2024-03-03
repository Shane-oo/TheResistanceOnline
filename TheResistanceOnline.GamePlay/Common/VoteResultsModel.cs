namespace TheResistanceOnline.GamePlay.Common;

public class VoteResultsModel
{
    #region Properties

    public Dictionary<string, bool> PlayerNameToVoteApproved { get; }

    public bool VoteSuccessful { get; }

    #endregion

    #region Construction

    public VoteResultsModel(Dictionary<string, bool> playerNameToVoteApproved, bool voteSuccessful)
    {
        PlayerNameToVoteApproved = playerNameToVoteApproved;
        VoteSuccessful = voteSuccessful;
    }

    #endregion
}
