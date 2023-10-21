using TheResistanceOnline.Core.Requests.Commands;
using TheResistanceOnline.GamePlay.GameModels;

namespace TheResistanceOnline.Hubs.Resistance.SelectMissionTeamPlayer;

public class SelectMissionTeamPlayerCommand: CommandBase
{
    #region Properties

    public string CallerPlayerName { get; set; }

    public GameModel GameModel { get; set; }

    public string LobbyId { get; set; }

    public string PlayerName { get; set; }

    #endregion
}
