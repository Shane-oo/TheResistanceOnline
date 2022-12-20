using AutoMapper;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using TheResistanceOnline.BusinessLogic.Core.Queries;
using TheResistanceOnline.BusinessLogic.DiscordServer;
using TheResistanceOnline.BusinessLogic.Games;
using TheResistanceOnline.BusinessLogic.Games.Commands;
using TheResistanceOnline.BusinessLogic.Games.Models;
using TheResistanceOnline.BusinessLogic.Users;
using TheResistanceOnline.BusinessLogic.Users.Models;
using TheResistanceOnline.Data.Users;

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

        private static readonly Dictionary<string, string?> _connectionIdToGroupNameMappingTable = new();
        private static readonly Dictionary<string, PlayerDetailsModel> _connectionIdToPlayerDetailsMappingTable = new();

        private readonly IDiscordServerService _discordServerService;
        private readonly IGameService _gameService;

        private static readonly Dictionary<string, GameDetailsModel> _groupNameToGameDetailsMappingTable = new()
                                                                                                           {
                                                                                                               {
                                                                                                                   "game-1", new GameDetailsModel
                                                                                                                       {
                                                                                                                           ChannelName = "game-1",
                                                                                                                           IsVoiceChannel = false,
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
                                                                                                                           IsVoiceChannel = false,
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
                                                                                                                           IsVoiceChannel = false,
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
                                                                                                                           IsVoiceChannel = false,
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
                                                                                                                           IsVoiceChannel = false,
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
                                                                                                                           IsVoiceChannel = true,
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
                                                                                                                           IsVoiceChannel = true,
                                                                                                                           IsAvailable = true,
                                                                                                                           PlayersDetails = new List<PlayerDetailsModel>()
                                                                                                                       }
                                                                                                               },
                                                                                                               {
                                                                                                                   "game-8", new GameDetailsModel
                                                                                                                       {
                                                                                                                           ChannelName = "game-8",
                                                                                                                           IsVoiceChannel = true,
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
                                                                                                                           IsVoiceChannel = true,
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
                                                                                                                           IsVoiceChannel = true,
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

        private readonly IUserService _userService;

        #endregion

        #region Construction

        public TheResistanceHub(IUserService userService, IDiscordServerService discordServerService, IGameService gameService, IMapper mapper)
        {
            _userService = userService;
            _discordServerService = discordServerService;
            _mapper = mapper;
            _gameService = gameService;
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


        private void ReceiveSubmitContinue(GameDetailsModel gameDetails,
                                           PlayerDetailsModel playerDetails,
                                           PlayerDetailsModel receivedPlayerDetails)
        {
            playerDetails.Continued = receivedPlayerDetails.Continued;

            _gameService.ProcessContinue(gameDetails);
        }

        private void ReceiveSubmitMissionChoice(GameDetailsModel gameDetails,
                                                PlayerDetailsModel playerDetails,
                                                PlayerDetailsModel receivedPlayerDetails)
        {
            playerDetails.Chose = receivedPlayerDetails.Chose;
            playerDetails.SupportedMission = receivedPlayerDetails.SupportedMission;

            _gameService.ProcessMission(gameDetails);
        }


        private void ReceiveSubmitMissionPropose(GameDetailsModel gameDetails, GameDetailsModel receivedGameDetails)
        {
            gameDetails.MissionTeam = receivedGameDetails.MissionTeam;
            gameDetails.GameStage = GameStageModel.Vote;
        }

        private void ReceiveSubmitVote(GameDetailsModel gameDetails,
                                       PlayerDetailsModel playerDetails,
                                       PlayerDetailsModel receivedPlayerDetails)
        {
            playerDetails.Voted = receivedPlayerDetails.Voted;
            playerDetails.ApprovedMissionTeam = receivedPlayerDetails.ApprovedMissionTeam;

            _gameService.ProcessVote(gameDetails);
        }

        private void RemoveConnectionFromAllMaps(string connectionId)
        {
            _connectionIdToGroupNameMappingTable.Remove(connectionId);
            _connectionIdToPlayerDetailsMappingTable.Remove(Context.ConnectionId);
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

        private async void SendDiscordNotFoundIfDiscordNotFoundAsync(User user)
        {
            // Discord User Does not exist Or User Wants to Use Discord And its been one week since said no 
            if (user.DiscordUser == null)
            {
                if (user.UserSetting.UserWantsToUseDiscord)
                {
                    await Clients.Client(Context.ConnectionId).SendAsync("ReceiveDiscordNotFound");
                }
                else if (!user.UserSetting.UserWantsToUseDiscordRecord.HasValue || user.UserSetting.UserWantsToUseDiscordRecord.Value <= DateTimeOffset.Now.AddDays(-7))
                {
                    await Clients.Client(Context.ConnectionId).SendAsync("ReceiveDiscordNotFound");
                }
            }
        }

        private async void SendGameDetailsToChannelGroupAsync(GameDetailsModel gameDetails, string groupName)
        {
            await Clients.Group(groupName).SendAsync("ReceiveGameDetails", gameDetails);

            var host = gameDetails.PlayersDetails?.FirstOrDefault(p => !p.IsBot);

            if (host != null) await Clients.Client(host.ConnectionId).SendAsync("ReceiveGameHost", true);
        }

        // call this function when game timer is finished
        // todo when hub is receiving commands from clients perform check for IsFinished and if true call this function
        private async Task SendGameFinishedToGroupAsync(string groupName)
        {
            //todo save all details before its deletion
            // this will refresh everyone in games page => triggering onDisconnectAsync() that will check for game over
            await Clients.Group(groupName).SendAsync("ReceiveGameFinished");
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
            var user = await _userService.GetUserByEmailOrNameAsync(new ByIdAndNameQuery
                                                                    {
                                                                        Name = Context.User?.Identity?.Name
                                                                    });

            var userDetails = _mapper.Map<UserDetailsModel>(user);
            //todo add resistance wins and spy wins eventually
            var playerDetails = new PlayerDetailsModel
                                {
                                    ConnectionId = Context.ConnectionId,
                                    PlayerId = Guid.NewGuid(),
                                    UserName = userDetails.UserName,
                                    DiscordUserName = userDetails.DiscordUser?.Name,
                                    DiscordTag = userDetails.DiscordUser?.DiscordTag,
                                    ResistanceTeamWins = 420,
                                    SpyTeamWins = 69
                                };

            _connectionIdToPlayerDetailsMappingTable.Add(Context.ConnectionId, playerDetails);

            SendAllGameDetailsToPlayersNotInGameAsync();

            SendDiscordNotFoundIfDiscordNotFoundAsync(user);

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

                SendGameDetailsToChannelGroupAsync(gameDetails, userGroupName);
                SendAllGameDetailsToPlayersNotInGameAsync();

                if (!string.IsNullOrEmpty(leavingPlayerDetails.DiscordTag))
                {
                    _discordServerService.RemoveRoleFromUserAsync(userGroupName, leavingPlayerDetails.DiscordTag);
                }
            }
            else
            {
                RemoveConnectionFromAllMaps(Context.ConnectionId);
            }

            return base.OnDisconnectedAsync(exception);
        }

        [UsedImplicitly]
        public async void ReceiveGameActionCommand(GameActionCommand gameActionCommand)
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
                    ReceiveSubmitVote(gameDetails, playerDetails, receivedPlayerDetails);
                    break;

                case GameActionModel.SubmitContinue:
                    ReceiveSubmitContinue(gameDetails, playerDetails, receivedPlayerDetails);
                    break;

                case GameActionModel.SubmitMissionChoice:
                    ReceiveSubmitMissionChoice(gameDetails, playerDetails, receivedPlayerDetails);
                    break;
                
                case GameActionModel.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            SendGameDetailsToChannelGroupAsync(gameDetails, groupName);
            Notify(gameDetails, groupName);
        }

        [UsedImplicitly]
        public async Task ReceiveJoinGameCommand(JoinGameCommand command)
        {
            var gameDetails = _groupNameToGameDetailsMappingTable[command.ChannelName];
            if (gameDetails.PlayersDetails is { Count: < MAX_GAME_COUNT })
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, command.ChannelName);
                _connectionIdToGroupNameMappingTable.Add(Context.ConnectionId, command.ChannelName);

                var newPlayerDetails = _connectionIdToPlayerDetailsMappingTable[Context.ConnectionId];

                gameDetails.PlayersDetails.Add(newPlayerDetails);
                _groupNameToGameDetailsMappingTable[command.ChannelName] = gameDetails;

                SendPlayerIdToClientAsync(newPlayerDetails.PlayerId);
                SendAllGameDetailsToPlayersNotInGameAsync();
                SendGameDetailsToChannelGroupAsync(gameDetails, command.ChannelName);

                if (!string.IsNullOrEmpty(newPlayerDetails.DiscordTag))
                {
                    _discordServerService.AddRoleToUserAsync(command.ChannelName, newPlayerDetails.DiscordTag);
                }
            }
        }

        [UsedImplicitly]
        public void ReceiveStartGameCommand(StartGameCommand command)
        {
            // get group name and game details
            if (!_connectionIdToGroupNameMappingTable.TryGetValue(Context.ConnectionId, out var groupName)) return;
            if (groupName == null) return;
            if (!_groupNameToGameDetailsMappingTable.TryGetValue(groupName, out var gameDetails)) return;

            gameDetails.GameOptions = command.GameOptions;

            // create gameService observer
            var gameServiceObserver = new GameSubjectAndObserver();
            // create bot observers and attach them to game service which is also subject to bots 
            var botObservers = gameServiceObserver.CreateBotObservers(command.GameOptions.BotCount);

            // attach gameService observer to hub subject
            Attach(gameServiceObserver, groupName);

            // add bots to game details
            foreach(var botObserver in botObservers)
            {
                var botPlayersDetails = new PlayerDetailsModel
                                        {
                                            PlayerId = Guid.NewGuid(),
                                            IsBot = true,
                                            BotObserver = botObserver,
                                            UserName = botObserver.GetName() + "-" + gameDetails.PlayersDetails?.Count
                                        };

                botObserver.SetPlayerId(botPlayersDetails.PlayerId);

                gameDetails.PlayersDetails?.Add(botPlayersDetails);
            }


            gameDetails = _gameService.SetUpNewGame(gameDetails);

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
            SendGameDetailsToChannelGroupAsync(gameDetails, groupName);
            SendAllGameDetailsToPlayersNotInGameAsync();
        }

        #endregion
    }
}
