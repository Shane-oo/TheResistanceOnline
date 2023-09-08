using MediatR;
using Microsoft.AspNetCore.SignalR;
using TheResistanceOnline.Games.Lobbies;
using TheResistanceOnline.Games.Lobbies.CreateLobby;

namespace TheResistanceOnline.Server.Lobbies.CreateLobby;

public class CreateLobbyHandler: IRequestHandler<CreateLobbyCommand, Unit>
{
    #region Fields

    private readonly IHubContext<LobbyHub, ILobbyHub> _hubContext;

    #endregion

    #region Construction

    public CreateLobbyHandler(IHubContext<LobbyHub, ILobbyHub> hubContext)
    {
        _hubContext = hubContext;
    }

    #endregion

    #region Public Methods

    public async Task<Unit> Handle(CreateLobbyCommand request, CancellationToken cancellationToken)
    {
        // TODO THIS SHOULD NOT BE LOBBYHUB THIS SHOULD BE GAME HUB GROUPS
        await _hubContext.Groups.AddToGroupAsync(request.ConnectionId, request.RoomId, cancellationToken);

        if (!request.PrivateRoom)
        {
            // TODO LISTEN FOR THIS ON CLIENT SIDE "newPublicLobby"
            await _hubContext.Clients.All.NewPublicLobby(request.RoomId);
        }

        return default;
    }

    #endregion
}
