using TheResistanceOnline.GamePlay;
using TheResistanceOnline.GamePlay.GameModels;
using TheResistanceOnline.Server.Common;

namespace TheResistanceOnline.Server.Resistance;

public class GameDetails
{
    #region Properties

    public bool BotsAllowed { get; set; }

    public List<ConnectionModel> Connections { get; } = new();

    public bool GameCommenced { get; set; }

    public GameModel GameModel { get; set; }

    public GameType GameType { get; set; }

    public int InitialBotCount { get; set; }

    public DateTime TimeCreated { get; set; }

    #endregion
}
