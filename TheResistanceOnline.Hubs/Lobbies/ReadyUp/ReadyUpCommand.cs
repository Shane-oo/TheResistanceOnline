using System.Collections.Concurrent;
using MediatR;
using TheResistanceOnline.Core.Requests.Commands;
using TheResistanceOnline.Hubs.Lobbies.Common;

namespace TheResistanceOnline.Hubs.Lobbies.ReadyUp;

public class ReadyUpCommand: CommandBase<Unit>
{
    public string LobbyId { get; set; }

    public ConcurrentDictionary<string, LobbyDetailsModel> GroupNamesToLobby { get; set; }
}
