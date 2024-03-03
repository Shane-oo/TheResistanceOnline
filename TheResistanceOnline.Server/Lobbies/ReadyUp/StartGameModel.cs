using TheResistanceOnline.GamePlay;

namespace TheResistanceOnline.Server.Lobbies;

public class StartGameModel
{
    #region Properties

    public int? Bots { get; set; }

    public bool BotsAllowed { get; set; }

    public string LobbyId { get; set; }

    public int TotalPlayers { get; set; }

    public GameType Type { get; set; } // for now this will always be Resistance Classic, in future might be Avalon type so in future make enum

    public List<string> UserNames { get; set; }

    #endregion
}
