using TheResistanceOnline.Core.Requests.Commands;
using TheResistanceOnline.Games.Lobbies.Common;

namespace TheResistanceOnline.Games.Lobbies.JoinLobby;

public class JoinLobbyCommand: CommandBase<LobbyDetailsModel>
{
    #region Properties

    public Dictionary<string, LobbyDetailsModel> GroupNamesToLobby { get; set; }

    public string LobbyId { get; set; }

    #endregion
}
