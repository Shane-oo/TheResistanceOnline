using System.Collections.Concurrent;
using TheResistanceOnline.Core.Requests.Queries;
using TheResistanceOnline.Games.Lobbies.Common;

namespace TheResistanceOnline.Games.Lobbies.GetLobbies;

public class GetLobbiesQuery: QueryBase<List<LobbyDetailsModel>>
{
    #region Properties

    public ConcurrentDictionary<string, LobbyDetailsModel> GroupNamesToLobby { get; set; }

    #endregion
}
