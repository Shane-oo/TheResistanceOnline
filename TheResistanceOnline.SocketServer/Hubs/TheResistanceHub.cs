using AutoMapper;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using TheResistanceOnline.BusinessLogic.BotObservers;
using TheResistanceOnline.BusinessLogic.Games;
using TheResistanceOnline.BusinessLogic.Games.Commands;
using TheResistanceOnline.BusinessLogic.Games.Models;
using TheResistanceOnline.BusinessLogic.PlayerStatistics.Models;
using TheResistanceOnline.BusinessLogic.Users.DbQueries;
using TheResistanceOnline.Data;
using TheResistanceOnline.Data.Entities.UserEntities;
using TheResistanceOnline.Data.UserSettings;

namespace TheResistanceOnline.SocketServer.Hubs
{
    [Authorize]
    public class TheResistanceHub: Hub, IResistanceHubSubject
    {
        #region Constants

        private const int MAX_GAME_COUNT = 10;
        private const int MAX_PLAYER_COUNT = 10;

        #endregion

        #region Fields

        private readonly INaiveBayesClassifierService _bayesClassifierService;

        private static readonly Dictionary<string, string?> _connectionIdToGroupNameMappingTable = new();
        private static readonly Dictionary<string, PlayerDetailsModel> _connectionIdToPlayerDetailsMappingTable = new();

        private readonly IDataContext _context;

        private readonly IGameService _gameService;

        private static readonly Dictionary<string, GameDetailsModel> _groupNameToGameDetailsMappingTable = new()
                                                                                                           {
                                                                                                               {
                                                                                                                   "game-1", new GameDetailsModel
                                                                                                                       {
                                                                                                                           ChannelName = "game-1",
                                                                                                                           IsAvailable = true,
                                                                                                                           PlayersDetails = new List<PlayerDetailsModel>(),
                                                                                                                           GameStage = GameStageModel.GameStart,
                                                                                                                           GameAction = GameActionModel.None
                                                                                                                       }
                                                                                                               },
                                                                                                               {
                                                                                                                   "game-2", new GameDetailsModel
                                                                                                                       {
                                                                                                                           ChannelName = "game-2",
                                                                                                                           IsAvailable = true,
                                                                                                                           PlayersDetails = new List<PlayerDetailsModel>(),
                                                                                                                           GameStage = GameStageModel.GameStart,
                                                                                                                           GameAction = GameActionModel.None
                                                                                                                       }
                                                                                                               },
                                                                                                               {
                                                                                                                   "game-3", new GameDetailsModel
                                                                                                                       {
                                                                                                                           ChannelName = "game-3",
                                                                                                                           IsAvailable = true,
                                                                                                                           PlayersDetails = new List<PlayerDetailsModel>(),
                                                                                                                           GameStage = GameStageModel.GameStart,
                                                                                                                           GameAction = GameActionModel.None
                                                                                                                       }
                                                                                                               },
                                                                                                               {
                                                                                                                   "game-4", new GameDetailsModel
                                                                                                                       {
                                                                                                                           ChannelName = "game-4",
                                                                                                                           IsAvailable = true,
                                                                                                                           PlayersDetails = new List<PlayerDetailsModel>(),
                                                                                                                           GameStage = GameStageModel.GameStart,
                                                                                                                           GameAction = GameActionModel.None
                                                                                                                       }
                                                                                                               },
                                                                                                               {
                                                                                                                   "game-5", new GameDetailsModel
                                                                                                                       {
                                                                                                                           ChannelName = "game-5",
                                                                                                                           IsAvailable = true,
                                                                                                                           PlayersDetails = new List<PlayerDetailsModel>(),
                                                                                                                           GameStage = GameStageModel.GameStart,
                                                                                                                           GameAction = GameActionModel.None
                                                                                                                       }
                                                                                                               },
                                                                                                               {
                                                                                                                   "game-6", new GameDetailsModel
                                                                                                                       {
                                                                                                                           ChannelName = "game-6",
                                                                                                                           IsAvailable = true,
                                                                                                                           PlayersDetails = new List<PlayerDetailsModel>(),
                                                                                                                           GameStage = GameStageModel.GameStart,
                                                                                                                           GameAction = GameActionModel.None
                                                                                                                       }
                                                                                                               },
                                                                                                               {
                                                                                                                   "game-7", new GameDetailsModel
                                                                                                                       {
                                                                                                                           ChannelName = "game-7",
                                                                                                                           IsAvailable = true,
                                                                                                                           PlayersDetails = new List<PlayerDetailsModel>()
                                                                                                                       }
                                                                                                               },
                                                                                                               {
                                                                                                                   "game-8", new GameDetailsModel
                                                                                                                       {
                                                                                                                           ChannelName = "game-8",
                                                                                                                           IsAvailable = true,
                                                                                                                           PlayersDetails = new List<PlayerDetailsModel>(),
                                                                                                                           GameStage = GameStageModel.GameStart,
                                                                                                                           GameAction = GameActionModel.None
                                                                                                                       }
                                                                                                               },
                                                                                                               {
                                                                                                                   "game-9", new GameDetailsModel
                                                                                                                       {
                                                                                                                           ChannelName = "game-9",
                                                                                                                           IsAvailable = true,
                                                                                                                           PlayersDetails = new List<PlayerDetailsModel>(),
                                                                                                                           GameStage = GameStageModel.GameStart,
                                                                                                                           GameAction = GameActionModel.None
                                                                                                                       }
                                                                                                               },
                                                                                                               {
                                                                                                                   "game-10", new GameDetailsModel
                                                                                                                       {
                                                                                                                           ChannelName = "game-10",
                                                                                                                           IsAvailable = true,
                                                                                                                           PlayersDetails = new List<PlayerDetailsModel>(),
                                                                                                                           GameStage = GameStageModel.GameStart,
                                                                                                                           GameAction = GameActionModel.None
                                                                                                                       }
                                                                                                               },
                                                                                                           };

        private static readonly Dictionary<string, IGameObserver> _groupNameToGameObserver = new();
        private static readonly Dictionary<string, Timer> _groupNameToGameTimer = new();
        private readonly IMapper _mapper;

        #endregion

        #region Construction

        public TheResistanceHub(IDataContext context,
                                IGameService gameService,
                                IMapper mapper,
                                INaiveBayesClassifierService bayesClassifierServiceService)
        {
            _context = context;
            _mapper = mapper;
            _gameService = gameService;
            _bayesClassifierService = bayesClassifierServiceService;
        }

        #endregion

        #region Private Methods

        private async Task<bool> CheckGameIsFinishedAsync(GameDetailsModel gameDetailsModel)
        {
            if (!gameDetailsModel.IsFinished) return false;
            await SendGameFinishedToGroupAsync(gameDetailsModel.ChannelName);
            return true;
        }

        private void CheckGameOver(GameDetailsModel gameDetails, string userGroupName)
        {
            if (gameDetails.PlayersDetails != null && !gameDetails.IsAvailable && gameDetails.PlayersDetails.All(x => x.IsBot))
            {
                Detach(userGroupName);
                gameDetails.PlayersDetails = new List<PlayerDetailsModel>();
                gameDetails.IsAvailable = true;
                if (!_groupNameToGameTimer.TryGetValue(userGroupName, out var gameTimer)) return;
                gameTimer.Dispose();
                _groupNameToGameTimer.Remove(userGroupName);
            }
        }

        private async Task ReceiveSubmitContinueAsync(GameDetailsModel gameDetails,
                                                      PlayerDetailsModel playerDetails,
                                                      PlayerDetailsModel receivedPlayerDetails)
        {
            playerDetails.Continued = receivedPlayerDetails.Continued;

            await _gameService.ProcessContinueAsync(gameDetails);
        }

        private async Task ReceiveSubmitMissionChoiceAsync(GameDetailsModel gameDetails,
                                                           PlayerDetailsModel playerDetails,
                                                           PlayerDetailsModel receivedPlayerDetails)
        {
            playerDetails.Chose = receivedPlayerDetails.Chose;
            playerDetails.SupportedMission = receivedPlayerDetails.SupportedMission;

            await _gameService.ProcessMissionAsync(gameDetails);
        }


        private void ReceiveSubmitMissionPropose(GameDetailsModel gameDetails, GameDetailsModel receivedGameDetails)
        {
            gameDetails.MissionTeam = receivedGameDetails.MissionTeam;
            gameDetails.GameStage = GameStageModel.Vote;
        }

        private async Task ReceiveSubmitVoteAsync(GameDetailsModel gameDetails,
                                                  PlayerDetailsModel playerDetails,
                                                  PlayerDetailsModel receivedPlayerDetails)
        {
            playerDetails.Voted = receivedPlayerDetails.Voted;
            playerDetails.ApprovedMissionTeam = receivedPlayerDetails.ApprovedMissionTeam;

            await _gameService.ProcessVoteAsync(gameDetails);
        }

        private void RemoveConnectionFromAllMaps(string connectionId)
        {
            _connectionIdToGroupNameMappingTable.Remove(connectionId);
            _connectionIdToPlayerDetailsMappingTable.Remove(Context.ConnectionId);
        }

        private async void SendAllGameDetailsToPlayersNotInGameAsync()
        {
            foreach(var (connectionId, playerDetails) in _connectionIdToPlayerDetailsMappingTable)
            {
                if (playerDetails.IsInAGame) continue;

                var availableGames = _groupNameToGameDetailsMappingTable.Where(gd => gd.Value.IsAvailable && gd.Value.PlayersDetails?.Count < MAX_PLAYER_COUNT)
                                                                        .OrderByDescending(gd => gd.Value.PlayersDetails?.Count)
                                                                        .ToDictionary(p => p.Key, p => p.Value);
                await Clients.Client(connectionId).SendAsync("ReceiveAllGameDetailsToPlayersNotInGame", availableGames);
            }
        }

        private async void SendGameDetailsToGroupAsync(GameDetailsModel gameDetails, string groupName)
        {
            await Clients.Group(groupName).SendAsync("ReceiveGameDetails", gameDetails);

            var host = gameDetails.PlayersDetails?.FirstOrDefault(p => !p.IsBot);

            if (host != null) await Clients.Client(host.ConnectionId).SendAsync("ReceiveGameHost", true);
        }

        // call this function when game timer is finished
        // todo when hub is receiving commands from clients perform check for IsFinished and if true call this function
        private async Task SendGameFinishedToGroupAsync(string groupName)
        {
            await Clients.Group(groupName).SendAsync("ReceiveGameFinished");
        }

        private async void SendNewMessageToGroupAsync(MessageModel message, string groupName)
        {
            await Clients.Group(groupName).SendAsync("ReceiveMessage", message);
        }

        private async void SendPlayerIdToClientAsync(Guid playerId)
        {
            await Clients.Client(Context.ConnectionId).SendAsync("ReceivePlayerId", playerId);
        }


        private static void SetGameToFinished(object? groupName)
        {
            var groupNameString = groupName?.ToString();
            if (groupNameString != null)
            {
                if (!_groupNameToGameDetailsMappingTable.TryGetValue(groupNameString, out var gameDetails)) return;
                gameDetails.IsFinished = true;
            }
        }

        #endregion

        #region Public Methods

        // Subject Function
        public void Attach(IGameObserver observer, string groupName)
        {
            _groupNameToGameObserver.Add(groupName, observer);
        }

        // Subject Function
        public void Detach(string groupName)
        {
            if (!_groupNameToGameObserver.TryGetValue(groupName, out var observer)) return;
            observer.Dispose();
            _groupNameToGameObserver.Remove(groupName);
        }

        // Subject Function
        public void Notify(GameDetailsModel gameDetails, string groupName)
        {
            if (!_groupNameToGameObserver.TryGetValue(groupName, out var gameObserver)) return;
            gameObserver.Update(gameDetails);
        }

        public override async Task OnConnectedAsync()
        {
            var user = await _context.Query<IUserByNameOrEmailDbQuery>()
                                     .WithParams(Context.User?.Identity?.Name)
                                     .Include(new[]
                                              {
                                                  nameof(UserSetting),
                                                  nameof(User.PlayerStatistics)
                                              })
                                     .AsNoTracking()
                                     .ExecuteAsync(new CancellationToken());
            var playerStatisticDetails = _mapper.Map<PlayerStatisticDetailsModel>(user.PlayerStatistics);

            var playerDetails = new PlayerDetailsModel
                                {
                                    ConnectionId = Context.ConnectionId,
                                    PlayerId = Guid.Parse(user.Id),
                                    UserName = user.UserName,
                                    ResistanceTeamWins = playerStatisticDetails.ResistanceWins,
                                    SpyTeamWins = playerStatisticDetails.SpyWins
                                };


            _connectionIdToPlayerDetailsMappingTable.Add(Context.ConnectionId, playerDetails);

            SendAllGameDetailsToPlayersNotInGameAsync();

            await base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            if (_connectionIdToGroupNameMappingTable.TryGetValue(Context.ConnectionId, out var userGroupName))
            {
                // shouldn't be null but just in case 
                if (userGroupName == null)
                {
                    RemoveConnectionFromAllMaps(Context.ConnectionId);
                    return base.OnDisconnectedAsync(exception);
                }

                var leavingPlayerDetails = _connectionIdToPlayerDetailsMappingTable[Context.ConnectionId];

                var gameDetails = _groupNameToGameDetailsMappingTable[userGroupName];
                gameDetails.PlayersDetails?.Remove(leavingPlayerDetails);

                CheckGameOver(gameDetails, userGroupName);

                RemoveConnectionFromAllMaps(Context.ConnectionId);

                SendGameDetailsToGroupAsync(gameDetails, userGroupName);
                SendAllGameDetailsToPlayersNotInGameAsync();
            }
            else
            {
                RemoveConnectionFromAllMaps(Context.ConnectionId);
            }

            return base.OnDisconnectedAsync(exception);
        }

        [UsedImplicitly]
        public async Task ReceiveGameActionCommand(GameActionCommand gameActionCommand)
        {
            // get group name and game details
            if (!_connectionIdToGroupNameMappingTable.TryGetValue(Context.ConnectionId, out var groupName)) return;
            if (groupName == null) return;
            if (!_groupNameToGameDetailsMappingTable.TryGetValue(groupName, out var gameDetails)) return;

            if (await CheckGameIsFinishedAsync(gameDetails))
            {
                return;
            }

            if (!_connectionIdToPlayerDetailsMappingTable.TryGetValue(Context.ConnectionId, out var playerDetails)) return;

            var receivedGameDetails = gameActionCommand.GameDetails;
            var receivedGameAction = receivedGameDetails.GameAction;
            var receivedPlayerDetails = receivedGameDetails.PlayersDetails?.FirstOrDefault(p => p.PlayerId == playerDetails.PlayerId);
            if (receivedPlayerDetails == null) return;

            switch(receivedGameAction)
            {
                case GameActionModel.SubmitMissionPropose:
                    ReceiveSubmitMissionPropose(gameDetails, receivedGameDetails);
                    break;

                case GameActionModel.SubmitVote:
                    await ReceiveSubmitVoteAsync(gameDetails, playerDetails, receivedPlayerDetails);
                    break;

                case GameActionModel.SubmitContinue:
                    //await _bayesClassifierService.GetTrainingDataAsync();
                    await ReceiveSubmitContinueAsync(gameDetails, playerDetails, receivedPlayerDetails);
                    break;

                case GameActionModel.SubmitMissionChoice:
                    await ReceiveSubmitMissionChoiceAsync(gameDetails, playerDetails, receivedPlayerDetails);
                    break;

                case GameActionModel.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            SendGameDetailsToGroupAsync(gameDetails, groupName);
            Notify(gameDetails, groupName);
            // little fun with spy predictions and messages

            if (gameDetails.GameStage == GameStageModel.MissionResults && gameDetails.CurrentMissionRound >= 3)
            {
                foreach(var bot in gameDetails.PlayersDetails!.Where(p => p.IsBot))
                {
                    var message = new MessageModel
                                  {
                                      Name = bot.UserName,
                                      Text = bot.BotObserver.GetSpyPredictions()
                                  };
                    SendNewMessageToGroupAsync(message, groupName);
                }
            }
        }

        [UsedImplicitly]
        public async Task ReceiveJoinGameCommand(JoinGameCommand command)
        {
            var gameDetails = _groupNameToGameDetailsMappingTable[command.ChannelName];
            var newPlayerDetails = _connectionIdToPlayerDetailsMappingTable[Context.ConnectionId];

            if (gameDetails.PlayersDetails is { Count: < MAX_GAME_COUNT } && gameDetails.PlayersDetails.All(p => p.PlayerId != newPlayerDetails.PlayerId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, command.ChannelName);
                _connectionIdToGroupNameMappingTable.Add(Context.ConnectionId, command.ChannelName);

                gameDetails.PlayersDetails.Add(newPlayerDetails);
                _groupNameToGameDetailsMappingTable[command.ChannelName] = gameDetails;

                SendPlayerIdToClientAsync(newPlayerDetails.PlayerId);
                SendAllGameDetailsToPlayersNotInGameAsync();
                SendGameDetailsToGroupAsync(gameDetails, command.ChannelName);
            }
        }

        [UsedImplicitly]
        public void ReceiveMessageCommand(string text)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                // get group name and game details
                if (!_connectionIdToGroupNameMappingTable.TryGetValue(Context.ConnectionId, out var groupName)) return;
                if (groupName == null) return;
                if (!_connectionIdToPlayerDetailsMappingTable.TryGetValue(Context.ConnectionId, out var playerDetails)) return;

                var message = new MessageModel
                              {
                                  Name = playerDetails.UserName,
                                  Text = text
                              };
                SendNewMessageToGroupAsync(message, groupName);
            }
        }

        [UsedImplicitly]
        public async Task ReceiveStartGameCommand(StartGameCommand command)
        {
            // get group name and game details
            if (!_connectionIdToGroupNameMappingTable.TryGetValue(Context.ConnectionId, out var groupName)) return;
            if (groupName == null) return;
            if (!_groupNameToGameDetailsMappingTable.TryGetValue(groupName, out var gameDetails)) return;

            gameDetails.GameOptions = command.GameOptions;

            // create gameService observer - with this implementation _bayesClassifierService always has to be passed
            var trainingData = await _bayesClassifierService.GetTrainingDataAsync();
            var gameServiceObserver = new GameSubjectAndObserver(trainingData);
            // create bot observers and attach them to game service which is also subject to bots 
            // Always Create Spectator Bot
            var playerValuesSpectator = gameServiceObserver.CreatePlayerValuesSpectatorBotObserver();
            gameDetails.PlayerValuesSpectator = playerValuesSpectator;

            var gamePlayingBotObservers = gameServiceObserver.CreateGamePlayingBotObservers(command.GameOptions.BotCount);


            // add bots to game details
            foreach(var gamePlayingBotObserver in gamePlayingBotObservers)
            {
                var botPlayersDetails = new PlayerDetailsModel
                                        {
                                            PlayerId = Guid.NewGuid(),
                                            IsBot = true,
                                            BotObserver = gamePlayingBotObserver,
                                            UserName = gamePlayingBotObserver.GetName() + "-" + gameDetails.PlayersDetails?.Count
                                        };

                gamePlayingBotObserver.SetPlayerId(botPlayersDetails.PlayerId);

                gameDetails.PlayersDetails?.Add(botPlayersDetails);
            }


            gameDetails = _gameService.SetUpNewGame(gameDetails);

            // attach gameService observer to hub subject
            Attach(gameServiceObserver, groupName);
            Notify(gameDetails, groupName);

            // not sure what to do about this
            var leader = gameDetails.PlayersDetails?.FirstOrDefault(p => p.IsMissionLeader);
            if (leader is { IsBot: true })
            {
                gameDetails.MissionTeam = leader.BotObserver.GetMissionProposal();
                // skip Mission Propose on client side as bot has decided
                gameDetails.GameStage = GameStageModel.Vote;

                Notify(gameDetails, groupName);
            }

            // start game
            var gameTimer = new Timer(SetGameToFinished, groupName, command.GameOptions.TimeLimitMinutes * 60000, Timeout.Infinite);
            _groupNameToGameTimer.Add(groupName, gameTimer);

            gameDetails.IsAvailable = false;
            SendGameDetailsToGroupAsync(gameDetails, groupName);
            SendAllGameDetailsToPlayersNotInGameAsync();
        }

        #endregion
    }
}
