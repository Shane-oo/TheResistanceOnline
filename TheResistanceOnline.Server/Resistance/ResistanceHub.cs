using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using TheResistanceOnline.Core.Errors;
using TheResistanceOnline.Core.Exchange.Responses;
using TheResistanceOnline.GamePlay.Common;
using TheResistanceOnline.Server.Common;

namespace TheResistanceOnline.Server.Resistance;

public interface IResistanceHub: IErrorHub
{
    public Task CommenceGame(CommenceGameModel commenceGameModel);

    public Task NewMissionTeamMember(string playerName);

    public Task RemoveMissionTeamMember(string playerName);

    public Task ShowMissionTeamSubmit(bool show);

    public Task VoteForMissionTeam(IEnumerable<string> missionTeamMembers);

    //public Task ShowVotes(a list of objects with playerName and vote choice)
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

    public ResistanceHub(IMediator mediator, ResistanceHubPersistedProperties properties, ILogger<ResistanceHub> logger): base(logger)
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

    private Result<GameDetails> GetGameDetails(string lobbyId)
    {
        _properties._groupNamesToGameModels.TryGetValue(lobbyId, out var gameDetails);

        var notFoundResult = NotFoundError.FailIfNull(lobbyId);
        return notFoundResult.IsFailure ? Result.Failure<GameDetails>(notFoundResult.Error) : Result.Success(gameDetails);
    }

    private Result<string> GetLobbyId()
    {
        _properties._connectionIdsToGroupNames.TryGetValue(Context.ConnectionId, out var lobbyId);
        var notFoundResult = NotFoundError.FailIfNull(lobbyId);
        return notFoundResult.IsFailure ? Result.Failure<string>(notFoundResult.Error) : Result.Success(lobbyId);
    }

    private async Task MissionTeamPlayerSelected(SelectMissionTeamPlayerCommand command)
    {
        SetRequest(command);
        SetCommand(command);

        var result = await _mediator.Send(command);
        if (result.IsFailure)
        {
            await Clients.Caller.Error(result.Error);
        }
    }

    #endregion

    #region Public Methods

    [UsedImplicitly]
    public async Task ObjectSelected(string name)
    {
        try
        {
            var lobbyId = GetLobbyId();
            if (lobbyId.IsFailure)
            {
                await Clients.Caller.Error(lobbyId.Error);
                return;
            }

            var gameDetails = GetGameDetails(lobbyId.Value);
            if (gameDetails.IsFailure)
            {
                await Clients.Caller.Error(gameDetails.Error);
                return;
            }

            switch(gameDetails.Value.GameModel.Phase)
            {
                case Phase.MissionBuild:
                    var command = new SelectMissionTeamPlayerCommand
                                  {
                                      SelectedPlayerName = name,
                                      GameModel = gameDetails.Value.GameModel,
                                      LobbyId = lobbyId.Value,
                                      CallerPlayerName = GetCallerPlayerName(gameDetails.Value)
                                  };
                    await MissionTeamPlayerSelected(command);
                    break;
                case Phase.Vote:
                    //todo
                    var callerPlayerName = GetCallerPlayerName(gameDetails.Value);
                    Console.WriteLine($"{callerPlayerName} voted: ${name}");
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
        catch(Exception ex)
        {
            await HandleError(ex);
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
                if (gameDetails.Connections.Count == 0)
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
        try
        {
            var gameDetails = GetGameDetails(command.LobbyId);
            if (gameDetails.IsFailure)
            {
                await Clients.Caller.Error(gameDetails.Error);
                return;
            }

            SetRequest(command);
            SetCommand(command);
            command.GameDetails = gameDetails.Value;

            var commenceGameResult = await _mediator.Send(command);
            if (commenceGameResult.IsFailure)
            {
                await Clients.Caller.Error(commenceGameResult.Error);
                return;
            }

            var commenceGame = commenceGameResult.Value;
            if (commenceGame)
            {
                var commenceGameCommand = new CommenceGameCommand
                                          {
                                              LobbyId = command.LobbyId,
                                              GameDetails = gameDetails.Value
                                          };
                var result = await _mediator.Send(commenceGameCommand);
                if (result.IsFailure)
                {
                    await Clients.Caller.Error(result.Error);
                }
            }
            // else{}
            // todo set a timer where if game hasnt commenced within like 2 minutes then its
            // a dead game and delete the details from all the maps too bad everyone
            // also set a timer on client side where if they havent got a commence game in 3 minutes
            // then refresh page as its a dead lobby
        }
        catch(Exception ex)
        {
            await HandleError(ex);
            throw;
        }
    }

    [UsedImplicitly]
    public async Task SubmitMissionTeam()
    {
        try
        {
            var lobbyId = GetLobbyId();
            if (lobbyId.IsFailure)
            {
                await Clients.Caller.Error(lobbyId.Error);
                return;
            }

            var gameDetails = GetGameDetails(lobbyId.Value);
            if (gameDetails.IsFailure)
            {
                await Clients.Caller.Error(gameDetails.Error);
                return;
            }

            var command = new SubmitMissionTeamCommand(lobbyId.Value, gameDetails.Value.GameModel);
            var result = await _mediator.Send(command);
            if (result.IsFailure)
            {
                await Clients.Caller.Error(result.Error);
            }
        }
        catch(Exception ex)
        {
            await HandleError(ex);
            throw;
        }
    }

    #endregion
}
