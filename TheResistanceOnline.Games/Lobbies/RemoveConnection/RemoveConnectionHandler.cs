using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace TheResistanceOnline.Games.Lobbies.RemoveConnection;

public class RemoveConnectionHandler: IRequestHandler<RemoveConnectionCommand, Unit>
{
    #region Fields

    private readonly IHubContext<LobbyHub, ILobbyHub> _lobbyHubContext;

    #endregion

    #region Construction

    public RemoveConnectionHandler(IHubContext<LobbyHub, ILobbyHub> lobbyHubContext)
    {
        _lobbyHubContext = lobbyHubContext;
    }

    #endregion

    #region Public Methods

    public async Task<Unit> Handle(RemoveConnectionCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        foreach(var lobby in command.LobbiesToRemoveFrom)
        {
            // if host left the lobby then the lobby must close
            if (lobby.HostConnectionId == command.ConnectionId)
            {
                command.GroupNamesToLobby.Remove(lobby.Id);

                if (!lobby.IsPrivate)
                {
                    // tell all clients not in groups that this lobby is no longer available
                    var allConnectionsInLobbies = command.GroupNamesToLobby.Values
                                                         .SelectMany(c => c.Connections)
                                                         .Select(c => c.ConnectionId)
                                                         .ToList();

                    await _lobbyHubContext.Clients.AllExcept(allConnectionsInLobbies).RemovePublicLobby(lobby.Id);
                }

                if (lobby.Connections.Any())
                {
                    // if there was anyone else in the lobby
                    // tell them that its closed and then remove them from the group
                    await _lobbyHubContext.Clients.Group(lobby.Id).LobbyClosed();
                    foreach(var connection in lobby.Connections)
                    {
                        await _lobbyHubContext.Groups.RemoveFromGroupAsync(lobby.Id, connection.ConnectionId, cancellationToken);
                    }
                }
            }
            else
            {
                lobby.Connections.RemoveAll(c => c.ConnectionId == command.ConnectionId);

                await _lobbyHubContext.Clients.Group(lobby.Id).RemoveConnectionInLobby(command.ConnectionId);

                if (!lobby.IsPrivate)
                {
                    var allConnectionsInLobbies = command.GroupNamesToLobby.Values
                                                         .SelectMany(c => c.Connections)
                                                         .Select(c => c.ConnectionId)
                                                         .ToList();

                    await _lobbyHubContext.Clients.AllExcept(allConnectionsInLobbies).UpdatePublicLobby(lobby);
                }
            }
        }

        return default;
    }

    #endregion
}
