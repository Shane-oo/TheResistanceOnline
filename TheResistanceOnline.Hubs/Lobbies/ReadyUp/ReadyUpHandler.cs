using MediatR;
using Microsoft.AspNetCore.SignalR;
using TheResistanceOnline.Core.Exceptions;
using TheResistanceOnline.Data.Entities.UserEntities;
using TheResistanceOnline.GamePlay;
using TheResistanceOnline.Games.Lobbies;
using TheResistanceOnline.Games.Lobbies.ReadyUp;

namespace TheResistanceOnline.Hubs.Lobbies.ReadyUp;

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

        if (!command.GroupNamesToLobby.TryGetValue(command.LobbyId, out var lobbyDetails))
        {
            throw new DomainException("Lobby With That Id Was Not Found");
        }

        var connection = lobbyDetails.Connections.FirstOrDefault(c => c.ConnectionId == command.ConnectionId);
        if (connection is null) throw new NotFoundException();

        if (connection.IsReady)
        {
            return default; // connection has already readied up
        }

        connection.IsReady = true;

        switch(lobbyDetails.FillWithBots)
        {
            case false when lobbyDetails.Connections.Count(c => c.IsReady) >= 5:
            {
                // if its all real players only start game when min player count reached

                var startGame = new StartGameModel
                                {
                                    LobbyId = Guid.NewGuid().ToString(), // send lobby id as guid so never to have two games going with same id
                                    UserNames = lobbyDetails.Connections.Select(c => c.UserName).ToList(),
                                    TotalPlayers = lobbyDetails.Connections.Count,
                                    Type = GameType.ResistanceClassic
                                };

                await _lobbyHubContext.Clients.Group(lobbyDetails.Id).StartGame(startGame);
                break;
            }
            case true when lobbyDetails.Connections.All(c => c.IsReady) || lobbyDetails.Connections.Count(c => c.IsReady) >= 5:
            {
                var botCount = lobbyDetails.MaxPlayers - lobbyDetails.Connections.Count;
                var startGame = new StartGameModel
                                {
                                    LobbyId = Guid.NewGuid().ToString(), // send lobby id as guid so never to have two games going with same id
                                    UserNames = lobbyDetails.Connections.Select(c => c.UserName).ToList(),
                                    BotsAllowed = true,
                                    Bots = botCount,
                                    TotalPlayers = lobbyDetails.Connections.Count + botCount,
                                    Type = GameType.ResistanceClassic
                                };
                await _lobbyHubContext.Clients.Group(lobbyDetails.Id).StartGame(startGame);
                break;
            }
            default:
                await _lobbyHubContext.Clients.Group(lobbyDetails.Id).UpdateConnectionsReadyInLobby(command.ConnectionId);
                break;
        }

        return default;
    }

    #endregion
}
