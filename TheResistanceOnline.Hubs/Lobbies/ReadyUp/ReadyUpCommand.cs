using System.Collections.Concurrent;
using MediatR;
using TheResistanceOnline.Core.Requests.Commands;


namespace TheResistanceOnline.Hubs.Lobbies;

public class ReadyUpCommand: CommandBase<Unit>
{
    public string LobbyId { get; set; }

    public ConcurrentDictionary<string, LobbyDetailsModel> GroupNamesToLobby { get; set; }
}
