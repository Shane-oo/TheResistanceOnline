using System.Collections.Concurrent;
using TheResistanceOnline.Core.Requests.Commands;
using TheResistanceOnline.Hubs.Lobbies.Common;

namespace TheResistanceOnline.Hubs.Lobbies.JoinLobby;

public class JoinLobbyCommand: CommandBase<LobbyDetailsModel>
{
    #region Properties

    public ConcurrentDictionary<string, LobbyDetailsModel> GroupNamesToLobby { get; set; }

    public string LobbyId { get; set; }

    #endregion
}
