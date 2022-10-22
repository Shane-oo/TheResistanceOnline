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

        private static readonly Dictionary<string, GameDetailsModel> _groupNameToGameDetailsMappingTable = new()
                                                                                                           {
                                                                                                               {
                                                                                                                   "game-1", new GameDetailsModel
                                                                                                                       {
                                                                                                                           ChannelName = "game-1",
                                                                                                                           IsVoiceChannel = false,
                                                                                                                           IsAvailable = true,
                                                                                                                           PlayersDetails = new List<PlayerDetailsModel>()
                                                                                                                       }
                                                                                                               },
                                                                                                               {
                                                                                                                   "game-2", new GameDetailsModel
                                                                                                                       {
                                                                                                                           ChannelName = "game-2",
                                                                                                                           IsVoiceChannel = false,
                                                                                                                           IsAvailable = true,
                                                                                                                           PlayersDetails = new List<PlayerDetailsModel>()
                                                                                                                       }
                                                                                                               },
                                                                                                               {
                                                                                                                   "game-3", new GameDetailsModel
                                                                                                                       {
                                                                                                                           ChannelName = "game-3",
                                                                                                                           IsVoiceChannel = false,
                                                                                                                           IsAvailable = true,
                                                                                                                           PlayersDetails = new List<PlayerDetailsModel>()
                                                                                                                       }
                                                                                                               },
                                                                                                               {
                                                                                                                   "game-4", new GameDetailsModel
                                                                                                                       {
                                                                                                                           ChannelName = "game-4",
                                                                                                                           IsVoiceChannel = false,
                                                                                                                           IsAvailable = true,
                                                                                                                           PlayersDetails = new List<PlayerDetailsModel>()
                                                                                                                       }
                                                                                                               },
                                                                                                               {
                                                                                                                   "game-5", new GameDetailsModel
                                                                                                                       {
                                                                                                                           ChannelName = "game-5",
                                                                                                                           IsVoiceChannel = false,
                                                                                                                           IsAvailable = true,
                                                                                                                           PlayersDetails = new List<PlayerDetailsModel>()
                                                                                                                       }
                                                                                                               },
                                                                                                               {
                                                                                                                   "game-6", new GameDetailsModel
                                                                                                                       {
                                                                                                                           ChannelName = "game-6",
                                                                                                                           IsVoiceChannel = true,
                                                                                                                           IsAvailable = true,
                                                                                                                           PlayersDetails = new List<PlayerDetailsModel>()
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
                                                                                                                           PlayersDetails = new List<PlayerDetailsModel>()
                                                                                                                       }
                                                                                                               },
                                                                                                               {
                                                                                                                   "game-9", new GameDetailsModel
                                                                                                                       {
                                                                                                                           ChannelName = "game-9",
                                                                                                                           IsVoiceChannel = true,
                                                                                                                           IsAvailable = true,
                                                                                                                           PlayersDetails = new List<PlayerDetailsModel>()
                                                                                                                       }
                                                                                                               },
                                                                                                               {
                                                                                                                   "game-10", new GameDetailsModel
                                                                                                                       {
                                                                                                                           ChannelName = "game-10",
                                                                                                                           IsVoiceChannel = true,
                                                                                                                           IsAvailable = true,
                                                                                                                           PlayersDetails = new List<PlayerDetailsModel>()
                                                                                                                       }
                                                                                                               },
                                                                                                           };

        private static readonly Dictionary<string, IGameObserver> _groupNameToGameObserver = new();
        private readonly IMapper _mapper;

        private readonly List<string?> _randomBotNames = new()
                                                         {
                                                             "WALL - E", "R2D2", "K9", "Optimus Prime", "Rosie", "Bender", "C-3PO", "HAL 9000", "Data", "ASIMO", "The Terminator",
                                                             "Micro", "EVA", "RAM", "Sputnik", "Humanoid", "Chip", "Robo", "Robocop", "Alpha", "Beta", "Gamma", "Siri",
                                                             "Raspberry Pie", "AstroBoy", "Chappie", "Ultron", "Omega", "Hydra", "Pixels"
                                                         };


        private readonly IUserService _userService;

        #endregion

        #region Construction

        public TheResistanceHub(IUserService userService, IDiscordServerService discordServerService, IMapper mapper)
        {
            _userService = userService;
            _discordServerService = discordServerService;
            _mapper = mapper;
        }

        #endregion

        #region Private Methods

        private void CheckGameOver(GameDetailsModel gameDetails, string userGroupName)
        {
            if (gameDetails.PlayersDetails != null && !gameDetails.IsAvailable && gameDetails.PlayersDetails.All(x => x.IsBot))
            {
                Detach(userGroupName);
                gameDetails.PlayersDetails = new List<PlayerDetailsModel>();
                gameDetails.IsAvailable = true;
            }
        }

        private string? GetRandomName()
        {
            return _randomBotNames.MinBy(x => Guid.NewGuid());
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

        private async void SendDiscordNotFoundAsync(User user)
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

            //todo send ReceiveGameHost : boolean to the host
            var host = gameDetails.PlayersDetails?.FirstOrDefault(p => !p.IsBot);

            if (host != null) await Clients.Client(host.ConnectionId).SendAsync("ReceiveGameHost", true);
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
            if (!_groupNameToGameObserver.TryGetValue(groupName, out var observer)) return;
            observer.Update(gameDetails);
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
                                    PlayerId = new Guid(),
                                    UserName = userDetails.UserName,
                                    DiscordUserName = userDetails.DiscordUser?.Name,
                                    DiscordTag = userDetails.DiscordUser?.DiscordTag,
                                    ResistanceTeamWins = 420,
                                    SpyTeamWins = 69
                                };

            _connectionIdToPlayerDetailsMappingTable.Add(Context.ConnectionId, playerDetails);

            SendAllGameDetailsToPlayersNotInGameAsync();

            SendDiscordNotFoundAsync(user);

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
            if (_connectionIdToGroupNameMappingTable.TryGetValue(Context.ConnectionId, out var groupName))
            {
                if (groupName != null)
                {
                    var gameService = new GameService();
                    var botObservers = gameService.CreateBotObservers(command.GameOptions.BotCount);

                    Attach(gameService, groupName);

                    if (_groupNameToGameDetailsMappingTable.TryGetValue(groupName, out var gameDetails))
                    {
                        foreach(var botObserver in botObservers)
                        {
                            gameDetails.PlayersDetails?.Add(new PlayerDetailsModel
                                                            {
                                                                IsBot = true,
                                                                BotObserver = botObserver,
                                                                UserName = GetRandomName()
                                                            });
                        }

                        Notify(gameDetails, groupName);

                        // start game
                        gameDetails.IsAvailable = false;
                        SendGameDetailsToChannelGroupAsync(gameDetails, groupName);
                        SendAllGameDetailsToPlayersNotInGameAsync();
                    }
                }
            }
        }

        #endregion
    }
}
