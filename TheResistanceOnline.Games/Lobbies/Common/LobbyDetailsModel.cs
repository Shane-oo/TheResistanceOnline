namespace TheResistanceOnline.Games.Lobbies.Common;

public class LobbyDetailsModel
{
    #region Properties

    public string HostConnectionId { get; set; }

    public string Id { get; set; }

    public bool IsPrivate { get; set; }

    public List<ConnectionModel> Connections { get; set; }

    #endregion
}
