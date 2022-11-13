namespace TheResistanceOnline.BusinessLogic.Games.Models;

public enum GameStageModel
{
    GameStart, // spies find out who the other spies are
    MissionPropose, // Mission Team Leader Selects the Mission Team
    Vote, // Everyone Votes on the proposed team for mission
    VoteResults, // Show the results from the most recent vote
    Mission, // If Vote Successful Mission Members go on mission
    MissionResults // Show the results from the most recent mission
}
