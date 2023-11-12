using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using TheResistanceOnline.Core.Exceptions;
using TheResistanceOnline.Hubs.Common;
using TheResistanceOnline.Hubs.Streams.Common;
using TheResistanceOnline.Hubs.Streams.GetConnectionIds;
using TheResistanceOnline.Hubs.Streams.JoinStream;
using TheResistanceOnline.Hubs.Streams.SendAnswer;
using TheResistanceOnline.Hubs.Streams.SendCandidate;
using TheResistanceOnline.Hubs.Streams.SendOffer;

namespace TheResistanceOnline.Hubs.Streams;

public interface IStreamHub: IErrorHub
{
    Task HandleAnswer(AnswerModel answer);

    Task HandleCandidate(CandidateModel candidate);

    Task HandleOffer(OfferModel offer);

    Task InitiateCall(ConnectionModel connection);

    Task NewConnectionId(ConnectionModel connection);

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

    #region Private Methods

    private string GetGroupName()
    {
        if (_properties._connectionIdsToGroupNames.TryGetValue(Context.ConnectionId, out var groupName))
        {
            return groupName;
        }

        throw new NotFoundException("Group Not Found");
    }

    private StreamGroupModel GetStreamGroup(string groupName)
    {
        if (_properties._groupNameToGroupModel.TryGetValue(groupName, out var streamGroupModel))
        {
            return streamGroupModel;
        }

        throw new NotFoundException("Group Model Not Found");
    }

    #endregion

    #region Public Methods

    [UsedImplicitly]
    public async Task<List<ConnectionModel>> GetConnectionIds(GetConnectionIdsQuery query)
    {
        SetRequest(query);
        try
        {
            var groupName = GetGroupName();
            query.StreamGroupModel = GetStreamGroup(groupName);
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
                await base.OnConnectedAsync();

                
                var command = new JoinStreamCommand
                              {
                                  LobbyId = lobbyId,
                                  ConnectionIdsToGroupNames = _properties._connectionIdsToGroupNames,
                                  GroupNameToGroupModel = _properties._groupNameToGroupModel
                              };
                SetRequest(command);

                await _mediator.Send(command);

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
            var groupName = GetGroupName();
            var groupModel = GetStreamGroup(groupName);
            await Clients.Group(lobbyId).RemoveConnectionId(Context.ConnectionId);
            _properties._connectionIdsToGroupNames.Remove(Context.ConnectionId, out _);

            groupModel.ConnectionIds.RemoveAll(c => c.ConnectionId == Context.ConnectionId);
            if (!groupModel.ConnectionIds.Any())
            {
                _properties._groupNameToGroupModel.Remove(groupName, out _);
            }
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
