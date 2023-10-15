using System.Collections.Concurrent;
using TheResistanceOnline.Hubs.Lobbies.Common;

namespace TheResistanceOnline.Hubs.Lobbies;

public class LobbyHubPersistedProperties
{
    #region Fields

    public readonly ConcurrentDictionary<string, LobbyDetailsModel> _groupNamesToLobby = new();

    #endregion
}
