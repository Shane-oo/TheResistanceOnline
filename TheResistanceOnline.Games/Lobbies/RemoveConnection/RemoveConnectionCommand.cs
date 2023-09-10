using MediatR;
using TheResistanceOnline.Core.Requests.Commands;
using TheResistanceOnline.Games.Lobbies.Common;

namespace TheResistanceOnline.Games.Lobbies.RemoveConnection;

public class RemoveConnectionCommand: CommandBase<Unit>
{
    #region Properties

    public Dictionary<string, LobbyDetailsModel> GroupNamesToLobby { get; set; }

    public List<LobbyDetailsModel> LobbiesToRemoveFrom { get; set; }

    #endregion
}
