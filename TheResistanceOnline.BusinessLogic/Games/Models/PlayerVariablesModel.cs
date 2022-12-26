using System.Text.Json.Serialization;

namespace TheResistanceOnline.BusinessLogic.Games.Models;

public class PlayerVariablesModel
{
    // Bayes Bot Variables
    // Variables After Missions
    [JsonIgnore]
    public int VotedForFailedMission { get; set; }

    [JsonIgnore]
    public int WentOnFailedMission { get; set; }

    [JsonIgnore]
    public int WentOnSuccessfulMission { get; set; }

    [JsonIgnore]
    public int ProposedTeamFailedMission { get; set; }

    [JsonIgnore]
    public int RejectedTeamSuccessfulMission { get; set; }

    // Voting Variables
    [JsonIgnore]
    public int VotedAgainstTeamProposal { get; set; }

    [JsonIgnore]
    public int VotedNoTeamHasSuccessfulMembers { get; set; }

    [JsonIgnore]
    public int VotedForTeamHasUnsuccessfulMembers { get; set; }

    [JsonIgnore]
    public int VotedForMissionNotOn { get; set; }

    // Proposed Team Variables
    [JsonIgnore]
    public int ProposedTeamHasUnsuccessfulMembers { get; set; }

    [JsonIgnore]
    public int ProposedTeamThatHaventBeenOnMissions { get; set; }
}
