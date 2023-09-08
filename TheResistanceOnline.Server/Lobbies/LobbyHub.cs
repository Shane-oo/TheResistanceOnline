using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using TheResistanceOnline.Data;
using TheResistanceOnline.Games.Lobbies.CreateLobby;
using TheResistanceOnline.Server.Hubs;

namespace TheResistanceOnline.Games.Lobbies;

public interface ILobbyHub
{
    Task NewPublicLobby(string roomId);
}

[Authorize]
public class LobbyHub: BaseHub<ILobbyHub>
{
    #region Fields

    private static readonly List<string> _connectionIds = new();
    private readonly IMediator _mediator;

    #endregion

    #region Construction

    public LobbyHub(IMediator mediator)
    {
        _mediator = mediator;
    }

    #endregion

    #region Public Methods

    public override async Task OnConnectedAsync()
    {
        _connectionIds.Add(Context.ConnectionId);

        await base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception exception)
    {
        _connectionIds.Remove(Context.ConnectionId);

        return base.OnDisconnectedAsync(exception);
    }

    [UsedImplicitly]
    public async Task ReceiveCreateLobbyCommand(CreateLobbyCommand command)
    {
        SetRequest(command);

        await _mediator.Send(command);

        throw new HubException("you messed up");
    }

    #endregion

    // Receive, Publish will be mediatr
}
