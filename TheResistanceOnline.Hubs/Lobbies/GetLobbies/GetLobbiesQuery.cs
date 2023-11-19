using System.Collections.Concurrent;
using TheResistanceOnline.Core.Requests.Queries;

namespace TheResistanceOnline.Hubs.Lobbies;

public class GetLobbiesQuery: QueryBase<List<LobbyDetailsModel>>
{
    #region Properties

    public ConcurrentDictionary<string, LobbyDetailsModel> GroupNamesToLobby { get; set; }

    #endregion
}
