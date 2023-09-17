using MediatR;
using Microsoft.AspNetCore.SignalR;
using TheResistanceOnline.Core.Exceptions;
using TheResistanceOnline.Data.Entities.UserEntities;

namespace TheResistanceOnline.Games.Lobbies.ReadyUp;

public class ReadyUpHandler: IRequestHandler<ReadyUpCommand, Unit>
{
    #region Fields

    private readonly IHubContext<LobbyHub, ILobbyHub> _lobbyHubContext;

    #endregion

    #region Construction

    public ReadyUpHandler(IHubContext<LobbyHub, ILobbyHub> lobbyHubContext)
    {
        _lobbyHubContext = lobbyHubContext;
    }

    #endregion

    #region Public Methods

    public async Task<Unit> Handle(ReadyUpCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        UnauthorizedException.ThrowIfUserIsNotAllowedAccess(command, Roles.User);

        if (!command.GroupNamesToLobby.TryGetValue(command.LobbyId, out var lobbyDetails))
        {
            throw new DomainException("Lobby With That Id Was Not Found");
        }

        var connection = lobbyDetails.Connections.FirstOrDefault(c => c.ConnectionId == command.ConnectionId);
        if (connection is null) throw new NotFoundException();

        connection.IsReady = true;

        if (!lobbyDetails.FillWithBots && lobbyDetails.Connections.Count(c => c.IsReady) >= 5)
        {
            // if its all real players only start game when min player count reached
            await _lobbyHubContext.Clients.Group(lobbyDetails.Id).StartGame();
        }
        else if (lobbyDetails.FillWithBots && (lobbyDetails.Connections.All(c => c.IsReady) || lobbyDetails.Connections.Count(c => c.IsReady) >= 5))
        {
            await _lobbyHubContext.Clients.Group(lobbyDetails.Id).StartGame();
        }
        else
        {
            await _lobbyHubContext.Clients.Group(lobbyDetails.Id).UpdateConnectionsReadyInLobby(command.ConnectionId);
        }

        return default;
    }

    #endregion
}
