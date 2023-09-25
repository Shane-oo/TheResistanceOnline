using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace TheResistanceOnline.Games.Resistance;

public interface IResistanceHub
{
}

public class ResistanceHub: BaseHub<IResistanceHub>
{
    private readonly IMediator _mediator;
    private readonly ResistanceHubPersistedProperties _properties;

    public ResistanceHub(IMediator mediator, ResistanceHubPersistedProperties properties)
    {
        _mediator = mediator;
        _properties = properties;
    }

    public override async Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();
        if (httpContext != null && httpContext.Request.Query.TryGetValue("lobbyId", out var lobbyQuery))
        {
            var lobbyId = lobbyQuery.FirstOrDefault();
            if (!string.IsNullOrEmpty(lobbyId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, lobbyId);
                _properties._connectionIdsToGroupNames[Context.ConnectionId] = lobbyId;

                await base.OnConnectedAsync();
            }
            else
            {
                throw new HubException("Missing LobbyId");
            }
        }
        else
        {
            throw new HubException("Missing LobbyId");
        }
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        if (_properties._connectionIdsToGroupNames.TryGetValue(Context.ConnectionId, out var lobbyId))
        {
            // todo let players know someone left, or just replace with bot idk yet
            //await Clients.Group(lobbyId).RemoveConnectionId(Context.ConnectionId);
            _properties._connectionIdsToGroupNames.Remove(Context.ConnectionId, out _);
        }

        await base.OnDisconnectedAsync(exception);
    }
}
