using System.Collections.Concurrent;
using TheResistanceOnline.Core.Requests.Commands;

namespace TheResistanceOnline.Hubs.Lobbies;

public class JoinLobbyCommand: CommandBase<string>
{
    #region Properties

    public ConcurrentDictionary<string, LobbyDetailsModel> GroupNamesToLobby { get; set; }

    public string LobbyId { get; set; }

    #endregion
}
