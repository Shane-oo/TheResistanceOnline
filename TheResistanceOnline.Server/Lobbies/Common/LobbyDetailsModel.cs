using TheResistanceOnline.Server.Common;

namespace TheResistanceOnline.Server.Lobbies;

public class LobbyDetailsModel
{
    #region Properties

    public List<ConnectionModel> Connections { get; set; }

    public bool FillWithBots { get; set; }

    public string HostConnectionId { get; set; }

    public string Id { get; set; }

    public bool IsPrivate { get; set; }

    public int MaxPlayers { get; set; }

    public DateTime TimeCreated { get; set; }

    #endregion
}
