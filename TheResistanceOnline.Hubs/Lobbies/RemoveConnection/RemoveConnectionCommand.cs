using System.Collections.Concurrent;
using MediatR;
using TheResistanceOnline.Core.Requests.Commands;

namespace TheResistanceOnline.Hubs.Lobbies;

public class RemoveConnectionCommand: CommandBase<Unit>
{
    #region Properties

    public ConcurrentDictionary<string, LobbyDetailsModel> GroupNamesToLobby { get; set; }

    public List<LobbyDetailsModel> LobbiesToRemoveFrom { get; set; }

    #endregion
}
