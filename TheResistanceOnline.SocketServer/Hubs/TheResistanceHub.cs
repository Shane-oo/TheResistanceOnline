using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using TheResistanceOnline.BusinessLogic.Core.Queries;
using TheResistanceOnline.BusinessLogic.Games;
using TheResistanceOnline.BusinessLogic.Games.Commands;
using TheResistanceOnline.BusinessLogic.Games.Models;
using TheResistanceOnline.BusinessLogic.Users;
using TheResistanceOnline.BusinessLogic.Users.Models;

namespace TheResistanceOnline.SocketServer.Hubs
{
    [Authorize]
    public class TheResistanceHub: Hub
    {
        #region Constants

        private const int MAX_GAME_COUNT = 10;

        #endregion

        #region Fields

        private static readonly Dictionary<string, string> _connectionIdToGroupNameMappingTable = new();
        private static readonly Dictionary<string, PlayerDetailsModel> _connectionIdToPlayerDetailsMappingTable = new();

        private static readonly Dictionary<string, Guid> _connectionIdToPlayerId = new();
        private readonly IGameService _gameService;

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

        private readonly IMapper _mapper;

        private readonly IUserService _userService;

        #endregion

        #region Construction

        public TheResistanceHub(IUserService userService, IGameService gameService, IMapper mapper)
        {
            _userService = userService;
            _gameService = gameService;
            _mapper = mapper;
        }

        #endregion

        #region Private Methods

        private PlayerDetailsModel? CreateNewPlayer()
        {
            var playerDetails = new PlayerDetailsModel
                                {
                                    PlayerId = Guid.NewGuid(),
                                    UserName = _connectionIdToPlayerDetailsMappingTable[Context.ConnectionId].UserName,
                                };
            if (string.IsNullOrEmpty(playerDetails.UserName))
            {
                return null;
            }

            _connectionIdToPlayerId.Add(Context.ConnectionId, playerDetails.PlayerId);
            return playerDetails;
        }

        private async Task SendAllGameDetailsToPlayersNotInGameAsync()
        {
            foreach(var (connectionId, playerDetails) in _connectionIdToPlayerDetailsMappingTable)
            {
                if (!playerDetails.IsInAGame)
                {
                    var availableGames = _groupNameToGameDetailsMappingTable.Where(gd => gd.Value.IsAvailable)
                                                                            .OrderByDescending(gd => gd.Value.PlayersDetails?.Count)
                                                                            .ToDictionary(p => p.Key, p => p.Value);
                    await Clients.Client(connectionId).SendAsync("ReceiveAllGameDetailsToPlayersNotInGame", availableGames);
                }
            }
        }

        #endregion

        #region Public Methods

        public async Task BroadcastMessageData(string message)
        {
            await Clients.All.SendAsync("broadcastmessagedata", message);
        }


        public async Task CreateGame(CreateGameCommand command)
        {
            var gameExists = _groupNameToGameDetailsMappingTable.ContainsKey(command.LobbyName);
            if (gameExists)
            {
                await Clients.Client(Context.ConnectionId).SendAsync("gameAlreadyExists", $"{command.LobbyName} Already Exists");
                return;
            }

            //todo this shouldnt be needed if picking from list of 10
            if (_groupNameToGameDetailsMappingTable.Count == MAX_GAME_COUNT)
            {
                await Clients.Client(Context.ConnectionId).SendAsync("tooManyGames", "There Exists Too Many Games Currently Playing. " +
                                                                                     "Please Wait Until Games Are Finished Before Creating A New Game");
                return;
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, command.LobbyName);
            _connectionIdToGroupNameMappingTable.Add(Context.ConnectionId, command.LobbyName);

            var playerDetails = CreateNewPlayer();
            if (playerDetails != null)
            {
                // _gameService.AssignRoleToPlayerAsync(command, _connectionIdToPlayerDetailsMappingTable[Context.ConnectionId]);

                // todo denote the host
                var newGame = new GameDetailsModel
                              {
                                  ChannelName = command.LobbyName,
                                  PlayersDetails = new List<PlayerDetailsModel>
                                                   {
                                                       playerDetails
                                                   }
                              };

                _groupNameToGameDetailsMappingTable.Add(command.LobbyName, newGame);

                await Clients.Group(command.LobbyName).SendAsync("userCreatedGame", newGame);
            }
        }

        public async Task JoinGame(JoinGameCommand command)
        {
            var gameExists = _groupNameToGameDetailsMappingTable.ContainsKey(command.LobbyName);
            if (gameExists)
            {
                var gameDetails = _groupNameToGameDetailsMappingTable[command.LobbyName];
                if (gameDetails.PlayersDetails is { Count: < 10 })
                {
                    var newPlayer = CreateNewPlayer();
                    if (newPlayer != null)
                    {
                        await Groups.AddToGroupAsync(Context.ConnectionId, command.LobbyName);
                        _connectionIdToGroupNameMappingTable.Add(Context.ConnectionId, command.LobbyName);


                        gameDetails.PlayersDetails.Add(newPlayer);
                        _groupNameToGameDetailsMappingTable[command.LobbyName] = gameDetails;

                        await Clients.Group(command.LobbyName).SendAsync("userJoinedGame", gameDetails);
                    }
                }
                else
                {
                    await Clients.Client(Context.ConnectionId).SendAsync("gameIsFull", "Game Is Full");
                }
            }
            else
            {
                await Clients.Client(Context.ConnectionId).SendAsync("gameDoesNotExist", "No Such Game Exists");
            }
        }

        public override async Task OnConnectedAsync()
        {
            var user = await _userService.GetUserByEmailOrNameAsync(new ByIdAndNameQuery
                                                                    {
                                                                        Name = Context.User?.Identity?.Name
                                                                    });
            var userDetails = _mapper.Map<UserDetailsModel>(user);
            var playerDetails = new PlayerDetailsModel
                                {
                                    PlayerId = new Guid(),
                                    UserName = userDetails.UserName,
                                    DiscordUserName = userDetails.DiscordUser?.UserName,
                                    DiscordTag = userDetails.DiscordUser?.DiscordTag,
                                };

            _connectionIdToPlayerDetailsMappingTable.Add(Context.ConnectionId, playerDetails);

            await SendAllGameDetailsToPlayersNotInGameAsync();

            // Discord User Does not exist Or User Wants to Use Discord And its been one week since said no 
            if (user.DiscordUser == null)
            {
                if (user.UserSetting.UserWantsToUseDiscord)
                {
                    await Clients.Client(Context.ConnectionId).SendAsync("discordNotFound");
                }
                else if (!user.UserSetting.UserWantsToUseDiscordRecord.HasValue || user.UserSetting.UserWantsToUseDiscordRecord.Value <= DateTimeOffset.Now.AddDays(-7))
                {
                    await Clients.Client(Context.ConnectionId).SendAsync("discordNotFound");
                }
            }


            await base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var userInGroup = _connectionIdToGroupNameMappingTable.ContainsKey(Context.ConnectionId);
            if (userInGroup)
            {
                var groupConnectionWasIn = _connectionIdToGroupNameMappingTable[Context.ConnectionId];
                var gamePlayerWasIn = _groupNameToGameDetailsMappingTable[groupConnectionWasIn];
                var playerId = _connectionIdToPlayerId[Context.ConnectionId];

                gamePlayerWasIn.PlayersDetails?.RemoveAll(p => p.PlayerId == playerId);

                Clients.Group(groupConnectionWasIn).SendAsync("userLeftGame", gamePlayerWasIn);

                _connectionIdToGroupNameMappingTable.Remove(Context.ConnectionId);
                _connectionIdToPlayerId.Remove(Context.ConnectionId);
            }

            _connectionIdToPlayerDetailsMappingTable.Remove(Context.ConnectionId);

            return base.OnDisconnectedAsync(exception);
        }

        #endregion
    }
}
