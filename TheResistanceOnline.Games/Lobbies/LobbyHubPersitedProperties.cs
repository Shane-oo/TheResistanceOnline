using System.Collections.Concurrent;
using TheResistanceOnline.Games.Lobbies.Common;

namespace TheResistanceOnline.Games.Lobbies;

public class LobbyHubPersistedProperties
{
    #region Fields

    public readonly ConcurrentDictionary<string, LobbyDetailsModel> _groupNamesToLobby = new();

    #endregion
}
