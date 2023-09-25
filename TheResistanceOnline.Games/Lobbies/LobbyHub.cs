using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using TheResistanceOnline.Games.Lobbies.Common;
using TheResistanceOnline.Games.Lobbies.CreateLobby;
using TheResistanceOnline.Games.Lobbies.GetLobbies;
using TheResistanceOnline.Games.Lobbies.JoinLobby;
using TheResistanceOnline.Games.Lobbies.ReadyUp;
using TheResistanceOnline.Games.Lobbies.RemoveConnection;
using TheResistanceOnline.Games.Lobbies.SearchLobby;

namespace TheResistanceOnline.Games.Lobbies;

public interface ILobbyHub
{
    Task Error(string errorMessage);

    Task LobbyClosed();

    Task NewConnectionInLobby(ConnectionModel connection);

    Task NewPublicLobby(LobbyDetailsModel lobby);

    Task RemoveConnectionInLobby(string connectionId);

    Task RemovePublicLobby(string id);

    Task UpdatePublicLobby(LobbyDetailsModel lobby);

    Task UpdateConnectionsReadyInLobby(string connectionId); // set ready to true for this connectionid 

    Task StartGame(StartGameModel startGame);
}

[Authorize]
public class LobbyHub: BaseHub<ILobbyHub>
{
    #region Fields

    private readonly LobbyHubPersistedProperties _properties;

    private readonly IMediator _mediator;

    #endregion

    #region Construction

    public LobbyHub(IMediator mediator, LobbyHubPersistedProperties properties)
    {
        _mediator = mediator;
        _properties = properties;
    }

    #endregion

    #region Public Methods

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

    [UsedImplicitly]
    public async Task<LobbyDetailsModel> CreateLobby(CreateLobbyCommand command)
    {
        SetRequest(command);
        command.GroupNamesToLobby = _properties._groupNamesToLobby;

        try
        {
            return await _mediator.Send(command);
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
    public async Task<LobbyDetailsModel> JoinLobby(JoinLobbyCommand command)
    {
        SetRequest(command);
        command.GroupNamesToLobby = _properties._groupNamesToLobby;

        try
        {
            return await _mediator.Send(command);
        }
        catch(Exception ex)
        {
            await Clients.Caller.Error(ex.Message);
            throw;
        }
    }

    [UsedImplicitly]
    public async Task<LobbyDetailsModel> SearchLobby(SearchLobbyQuery query)
    {
        SetRequest(query);
        query.GroupNamesToLobby = _properties._groupNamesToLobby;
        try
        {
            var foundLobbyId = await _mediator.Send(query);
            var command = new JoinLobbyCommand
                          {
                              LobbyId = foundLobbyId,
                              GroupNamesToLobby = _properties._groupNamesToLobby
                          };
            SetRequest(command);
            return await JoinLobby(command);
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

    #endregion
}
