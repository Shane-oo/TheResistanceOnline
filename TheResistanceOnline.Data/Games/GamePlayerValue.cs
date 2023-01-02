using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;

namespace TheResistanceOnline.Data.Games;

[UsedImplicitly]
[Table("GamePlayerValues")]
public class GamePlayerValue
{
    #region Properties

    public int Id { get; set; }

    public int GameId { get; set; }

    public Game Game { get; set; }

    // Array Of Jsons
    public List<PlayerValue> PlayerValues { get; set; }

    #endregion
}

// Json
[UsedImplicitly]
public class PlayerValue
{
    // Bayes Bot Variables
    // Variables After Missions
    public int VotedForFailedMission { get; set; }

    public int WentOnFailedMission { get; set; }

    public int WentOnSuccessfulMission { get; set; }     // not in og

    public int ProposedTeamFailedMission { get; set; }


    public int RejectedTeamSuccessfulMission { get; set; }

    // Voting Variables
    public int VotedAgainstTeamProposal { get; set; }

    public int VotedNoTeamHasSuccessfulMembers { get; set; }

    public int VotedForTeamHasUnsuccessfulMembers { get; set; }

    public int VotedForMissionNotOn { get; set; }

    // Proposed Team Variables
    public int ProposedTeamHasUnsuccessfulMembers { get; set; }

    public int ProposedTeamThatHaventBeenOnMissions { get; set; }
    

    // Final Variables
    public int Team { get; set; }     // 0  for Resistance 1 For Spies
}
