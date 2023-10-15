using System.Collections.Concurrent;
using TheResistanceOnline.Core.Requests.Queries;
using TheResistanceOnline.Hubs.Lobbies.Common;

namespace TheResistanceOnline.Hubs.Lobbies.GetLobbies;

public class GetLobbiesQuery: QueryBase<List<LobbyDetailsModel>>
{
    #region Properties

    public ConcurrentDictionary<string, LobbyDetailsModel> GroupNamesToLobby { get; set; }

    #endregion
}
