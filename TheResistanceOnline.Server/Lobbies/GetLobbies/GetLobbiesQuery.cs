using System.Collections.Concurrent;
using TheResistanceOnline.Core.NewCommandAndQueriesAndResultsPattern;

namespace TheResistanceOnline.Server.Lobbies;

public class GetLobbiesQuery: Query<List<LobbyDetailsModel>>
{
    #region Properties

    public ConcurrentDictionary<string, LobbyDetailsModel> GroupNamesToLobby { get; set; }

    #endregion
}
