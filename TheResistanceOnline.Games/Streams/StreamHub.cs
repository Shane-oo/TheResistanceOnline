using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using TheResistanceOnline.Games.Streams.CreateOffer;
using TheResistanceOnline.Games.Streams.GetPeerConnections;
using TheResistanceOnline.Games.Streams.SendAnswer;
using TheResistanceOnline.Games.Streams.SendCandidate;
using TheResistanceOnline.Games.Streams.SendOffer;

namespace TheResistanceOnline.Games.Streams;

public interface IStreamHub
{
    Task Error(string errorMessage);

    Task HandleAnswer(AnswerModel answer);

    Task HandleCandidate(CandidateModel candidate);

    Task HandleOffer(OfferModel offer);

    Task NewConnectionId(string connectionId);

    Task RemoveConnectionId(string connectionId);
}

public class StreamHub: BaseHub<IStreamHub>
{
    #region Fields

    private readonly IMediator _mediator;

    private readonly StreamHubPersistedProperties _properties;

    #endregion

    #region Construction

    public StreamHub(IMediator mediator, StreamHubPersistedProperties properties)
    {
        _mediator = mediator;
        _properties = properties;
    }

    #endregion

    #region Public Methods

    [UsedImplicitly]
    public async Task<List<string>> GetConnectionIds(GetConnectionIdsQuery query)
    {
        SetRequest(query);
        query.ConnectionIdsToGroupNames = _properties._connectionIdsToGroupNames;

        try
        {
            return await _mediator.Send(query);
        }
        catch(Exception ex)
        {
            await Clients.Caller.Error(ex.Message);
            throw;
        }
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
                _properties._connectionIdsToGroupNames.Add(Context.ConnectionId, lobbyId);

                // let others now of new connection
                await Clients.Group(lobbyId).NewConnectionId(Context.ConnectionId);

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
            await Clients.Group(lobbyId).RemoveConnectionId(Context.ConnectionId);
            _properties._connectionIdsToGroupNames.Remove(Context.ConnectionId);
        }

        await base.OnDisconnectedAsync(exception);
    }

    [UsedImplicitly]
    public async Task SendAnswer(SendAnswerCommand command)
    {
        SetRequest(command);
        try
        {
            await _mediator.Send(command);
        }
        catch(Exception ex)
        {
            await Clients.Caller.Error(ex.Message);
            throw;
        }
    }

    [UsedImplicitly]
    public async Task SendCandidate(SendCandidateCommand command)
    {
        SetRequest(command);
        command.ConnectionIdsToGroupNames = _properties._connectionIdsToGroupNames;

        try
        {
            await _mediator.Send(command);
        }
        catch(Exception ex)
        {
            await Clients.Caller.Error(ex.Message);
            throw;
        }
    }

    [UsedImplicitly]
    public async Task SendOffer(SendOfferCommand command)
    {
        SetRequest(command);
        try
        {
            await _mediator.Send(command);
        }
        catch(Exception ex)
        {
            await Clients.Caller.Error(ex.Message);
            throw;
        }
    }

    #endregion
}
