using System.Collections.Concurrent;
using TheResistanceOnline.Core.NewCommandAndQueriesAndResultsPattern;

namespace TheResistanceOnline.Server.Lobbies;

public class GetLobbyQuery: Query<LobbyDetailsModel>
{
    #region Properties

    public ConcurrentDictionary<string, LobbyDetailsModel> GroupNamesToLobby { get; set; }

    public string Id { get; set; }

    #endregion
}
