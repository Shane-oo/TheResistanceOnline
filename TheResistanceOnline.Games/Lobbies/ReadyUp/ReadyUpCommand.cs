using MediatR;
using TheResistanceOnline.Core.Requests.Commands;
using TheResistanceOnline.Games.Lobbies.Common;

namespace TheResistanceOnline.Games.Lobbies.ReadyUp;

public class ReadyUpCommand: CommandBase<Unit>
{
    public string LobbyId { get; set; }

    public Dictionary<string, LobbyDetailsModel> GroupNamesToLobby { get; set; }
}
