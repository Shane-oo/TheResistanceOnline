using System.Collections.Concurrent;

namespace TheResistanceOnline.Server.Lobbies;

public class LobbyHubPersistedProperties
{
    #region Fields

    public readonly ConcurrentDictionary<string, LobbyDetailsModel> _groupNamesToLobby = new();

    #endregion
}
