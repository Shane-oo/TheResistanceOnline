using System.Collections.Concurrent;

namespace TheResistanceOnline.Hubs.Lobbies;

public class LobbyHubPersistedProperties
{
    #region Fields

    public readonly ConcurrentDictionary<string, LobbyDetailsModel> _groupNamesToLobby = new();

    #endregion
}
