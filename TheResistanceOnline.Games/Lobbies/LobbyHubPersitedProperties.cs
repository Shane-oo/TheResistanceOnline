using TheResistanceOnline.Games.Lobbies.Common;

namespace TheResistanceOnline.Games.Lobbies;

public class LobbyHubPersistedProperties
{
    #region Fields

    public readonly Dictionary<string, LobbyDetailsModel> _groupNamesToLobby = new();

    #endregion
}
