using System.Collections.Concurrent;
using TheResistanceOnline.Core.Exchange.Requests;
using TheResistanceOnline.Server.Common;

namespace TheResistanceOnline.Server.Lobbies;

public class RemoveConnectionCommand: Command, IConnectionModel
{
    #region Properties

    public string ConnectionId { get; set; }

    public ConcurrentDictionary<string, LobbyDetailsModel> GroupNamesToLobby { get; set; }

    public List<LobbyDetailsModel> LobbiesToRemoveFrom { get; set; }

    #endregion
}
