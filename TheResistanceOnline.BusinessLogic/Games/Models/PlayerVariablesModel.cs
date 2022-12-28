using System.Text.Json.Serialization;

namespace TheResistanceOnline.BusinessLogic.Games.Models;

public class PlayerVariablesModel
{
    // Bayes Bot Variables
    // Variables After Missions
    public int VotedForFailedMission { get; set; }

    public int WentOnFailedMission { get; set; }

    public int WentOnSuccessfulMission { get; set; }

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
    public TeamModel Team { get; set; }
}
