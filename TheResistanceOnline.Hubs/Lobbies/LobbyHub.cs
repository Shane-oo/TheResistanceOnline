using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using TheResistanceOnline.Hubs.Common;

namespace TheResistanceOnline.Hubs.Lobbies;

public interface ILobbyHub: IErrorHub
{
    Task LobbyClosed();

    Task NewConnectionInLobby(ConnectionModel connection);

    Task NewPublicLobby(LobbyDetailsModel lobby);

    Task RemoveConnectionInLobby(string connectionId);

    Task RemovePublicLobby(string id);

    Task StartGame(StartGameModel startGame);

    Task UpdateConnectionsReadyInLobby(string connectionId); // set ready to true for this connectionid 

    Task UpdatePublicLobby(LobbyDetailsModel lobby);
}

[Authorize]
public class LobbyHub: BaseHub<ILobbyHub>
{
    #region Constants

    private const int LOBBY_TIME_TO_LIVE_MINUTES = 15;

    #endregion

    #region Fields

    private readonly IMediator _mediator;

    private readonly LobbyHubPersistedProperties _properties;

    #endregion

    #region Construction

    public LobbyHub(IMediator mediator, LobbyHubPersistedProperties properties)
    {
        _mediator = mediator;
        _properties = properties;
        CleanUp(); // Hubs are transient so the constructor is called with every request
    }

    #endregion

    #region Private Methods

    private void CleanUp()
    {
        var groupsToDelete = new List<string>();
        foreach(var groupNameToLobby in _properties._groupNamesToLobby)
        {
            if (groupNameToLobby.Value.TimeCreated.AddMinutes(LOBBY_TIME_TO_LIVE_MINUTES) <= DateTime.UtcNow)
            {
                groupsToDelete.Add(groupNameToLobby.Key);
            }
        }

        foreach(var groupToDelete in groupsToDelete)
        {
            _properties._groupNamesToLobby.TryRemove(groupToDelete, out _);
        }
    }

    #endregion

    #region Public Methods

    [UsedImplicitly]
    public async Task<LobbyDetailsModel> CreateLobby(CreateLobbyCommand command)
    {
        SetRequest(command);
        command.GroupNamesToLobby = _properties._groupNamesToLobby;

        try
        {
            var lobbyId = await _mediator.Send(command);
            var getLobbyQuery = new GetLobbyQuery
                                {
                                    Id = lobbyId,
                                };
            return await GetLobby(getLobbyQuery);
        }
        catch(Exception ex)
        {
            await Clients.Caller.Error(ex.Message);
            throw;
        }
    }

    [UsedImplicitly]
    public async Task<List<LobbyDetailsModel>> GetLobbies()
    {
        var query = new GetLobbiesQuery();
        SetRequest(query);
        query.GroupNamesToLobby = _properties._groupNamesToLobby;

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


    [UsedImplicitly]
    public async Task<LobbyDetailsModel> GetLobby(GetLobbyQuery query)
    {
        SetRequest(query);
        query.GroupNamesToLobby = _properties._groupNamesToLobby;

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

    [UsedImplicitly]
    public async Task<LobbyDetailsModel> JoinLobby(JoinLobbyCommand command)
    {
        SetRequest(command);
        command.GroupNamesToLobby = _properties._groupNamesToLobby;

        try
        {
            var lobbyId = await _mediator.Send(command);
            var getLobbyQuery = new GetLobbyQuery
                                {
                                    Id = lobbyId,
                                };
            return await GetLobby(getLobbyQuery);
        }
        catch(Exception ex)
        {
            await Clients.Caller.Error(ex.Message);
            throw;
        }
    }

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var matchingLobbies = _properties._groupNamesToLobby
                                         .Values
                                         .Where(l => l.Connections.Any(c => c.ConnectionId == Context.ConnectionId))
                                         .ToList();
        if (matchingLobbies.Any())
        {
            var command = new RemoveConnectionCommand
                          {
                              GroupNamesToLobby = _properties._groupNamesToLobby,
                              LobbiesToRemoveFrom = matchingLobbies
                          };
            SetRequest(command);
            await _mediator.Send(command);
        }

        await base.OnDisconnectedAsync(exception);
    }

    [UsedImplicitly]
    public async Task ReadyUp(ReadyUpCommand command)
    {
        SetRequest(command);
        command.GroupNamesToLobby = _properties._groupNamesToLobby;

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
