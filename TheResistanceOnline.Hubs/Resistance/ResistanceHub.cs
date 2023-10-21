using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using TheResistanceOnline.GamePlay.Common;
using TheResistanceOnline.Hubs.Common;
using TheResistanceOnline.Hubs.Resistance.CommenceGame;
using TheResistanceOnline.Hubs.Resistance.Common;
using TheResistanceOnline.Hubs.Resistance.SelectMissionTeamPlayer;
using TheResistanceOnline.Hubs.Resistance.StartGame;

namespace TheResistanceOnline.Hubs.Resistance;

public interface IResistanceHub: IErrorHub
{
    public Task CommenceGame(CommenceGameModel commenceGameModel);

    public Task NewMissionTeamMember(string playerName);

    public Task RemoveMissionTeamMember(string playerName);

    public Task ShowMissionTeamSubmit(bool show);
}

public class ResistanceHub: BaseHub<IResistanceHub>
{
    #region Constants

    private const int GAME_TIME_TO_LIVE_MINUTES = 60;

    #endregion

    #region Fields

    private readonly IMediator _mediator;
    private readonly ResistanceHubPersistedProperties _properties;

    #endregion

    #region Construction

    public ResistanceHub(IMediator mediator, ResistanceHubPersistedProperties properties)
    {
        _mediator = mediator;
        _properties = properties;
        CleanUp();
    }

    #endregion

    #region Private Methods

    private void CleanUp()
    {
        var groupsToDelete = new List<string>();
        foreach(var groupNameToLobby in _properties._groupNamesToGameModels)
        {
            if (groupNameToLobby.Value.TimeCreated.AddMinutes(GAME_TIME_TO_LIVE_MINUTES) <= DateTime.UtcNow)
            {
                groupsToDelete.Add(groupNameToLobby.Key);
            }
        }

        foreach(var groupToDelete in groupsToDelete)
        {
            _properties._groupNamesToGameModels.TryRemove(groupToDelete, out _);
        }
    }

    private string GetCallerPlayerName(GameDetails gameDetails)
    {
        return gameDetails.Connections.FirstOrDefault(c => c.ConnectionId == Context.ConnectionId)?.UserName;
    }

    private GameDetails GetGameDetails(string lobbyId)
    {
        if (!string.IsNullOrEmpty(lobbyId))
        {
            return _properties._groupNamesToGameModels.TryGetValue(lobbyId, out var gameDetails) ? gameDetails : null;
        }

        return null;
    }

    private string GetLobbyId()
    {
        return _properties._connectionIdsToGroupNames.TryGetValue(Context.ConnectionId, out var lobbyId) ? lobbyId : null;
    }

    private async Task MissionTeamPlayerSelected(SelectMissionTeamPlayerCommand command)
    {
        SetRequest(command);

        await _mediator.Send(command);
    }

    #endregion

    #region Public Methods

    [UsedImplicitly]
    public async Task ObjectSelected(string name)
    {
        var lobbyId = GetLobbyId();
        var gameDetails = GetGameDetails(lobbyId);
        if (gameDetails == null)
        {
            await Clients.Caller.Error("Game Not Found");
            return;
        }

        switch(gameDetails.GameModel.Phase)
        {
            case Phase.MissionBuild:
                var command = new SelectMissionTeamPlayerCommand
                              {
                                  PlayerName = name,
                                  GameModel = gameDetails.GameModel,
                                  LobbyId = lobbyId,
                                  CallerPlayerName = GetCallerPlayerName(gameDetails)
                              };
                await MissionTeamPlayerSelected(command);
                break;
            case Phase.Vote:
                break;
            case Phase.VoteResults:
                break;
            case Phase.Mission:
                break;
            case Phase.MissionResults:
                break;
            default:
                throw new ArgumentOutOfRangeException();
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
                _properties._connectionIdsToGroupNames[Context.ConnectionId] = lobbyId;

                var newGame = new GameDetails
                              {
                                  TimeCreated = DateTime.UtcNow
                              };
                _properties._groupNamesToGameModels.TryAdd(lobbyId, newGame);

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
            if (_properties._groupNamesToGameModels.TryGetValue(lobbyId, out var gameDetails))
            {
                gameDetails.Connections.RemoveAll(c => c.ConnectionId == Context.ConnectionId);
                if (!gameDetails.Connections.Any())
                {
                    _properties._groupNamesToGameModels.Remove(lobbyId, out _);
                }
            }

            _properties._connectionIdsToGroupNames.Remove(Context.ConnectionId, out _);
        }

        await base.OnDisconnectedAsync(exception);
    }

    [UsedImplicitly]
    public async Task StartGame(StartGameCommand command)
    {
        var gameDetails = GetGameDetails(command.LobbyId);
        if (gameDetails == null)
        {
            await Clients.Caller.Error("Game Not Found");
            return;
        }

        SetRequest(command);
        command.GameDetails = gameDetails;
        try
        {
            var canCommenceGame = await _mediator.Send(command);
            if (canCommenceGame)
            {
                var commenceGameCommand = new CommenceGameCommand
                                          {
                                              LobbyId = command.LobbyId,
                                              GameDetails = gameDetails
                                          };
                await _mediator.Send(commenceGameCommand);
            }
            // else{}
            // todo set a timer where if game hasnt commenced within like 2 minutes then its
            // a dead game and delete the details from all the maps too bad everyone
            // also set a timer on client side where if they havent got a commence game in 3 minutes
            // then refresh page as its a dead lobby
        }
        catch(Exception ex)
        {
            await Clients.Caller.Error(ex.Message);
            throw;
        }
    }

    #endregion
}
