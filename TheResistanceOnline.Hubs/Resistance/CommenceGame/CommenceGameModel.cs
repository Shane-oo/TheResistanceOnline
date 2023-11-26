using TheResistanceOnline.GamePlay.Common;

namespace TheResistanceOnline.Hubs.Resistance;

// The Initial Details for everyone to start game
public class CommenceGameModel
{
    #region Properties

    public bool IsMissionLeader { get; set; }

    public string MissionLeader { get; set; }

    public Phase Phase { get; set; }

    public List<string> Players { get; set; }

    public Team Team { get; set; }

    // If they are on team spy send who the other spies are to them
    public List<string> TeamMates { get; set; }

    #endregion
}
