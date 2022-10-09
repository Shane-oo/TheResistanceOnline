using AutoMapper;
using JetBrains.Annotations;
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

        private static readonly Dictionary<string, string?> _connectionIdToGroupNameMappingTable = new();
        private static readonly Dictionary<string, PlayerDetailsModel> _connectionIdToPlayerDetailsMappingTable = new();

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

                var availableGames = _groupNameToGameDetailsMappingTable.Where(gd => gd.Value.IsAvailable)
                                                                        .OrderByDescending(gd => gd.Value.PlayersDetails?.Count)
                                                                        .ToDictionary(p => p.Key, p => p.Value);
                await Clients.Client(connectionId).SendAsync("ReceiveAllGameDetailsToPlayersNotInGame", availableGames);
            }
        }

        private async void SendGameDetailsToChannelGroupAsync(GameDetailsModel gameDetails, string groupName)
        {
            await Clients.Group(groupName).SendAsync("ReceiveGameDetails", gameDetails);
        }

        #endregion

        #region Public Methods

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
                                    PlayerId = new Guid(),
                                    UserName = userDetails.UserName,
                                    DiscordUserName = userDetails.DiscordUser?.UserName,
                                    DiscordTag = userDetails.DiscordUser?.DiscordTag,
                                };

            _connectionIdToPlayerDetailsMappingTable.Add(Context.ConnectionId, playerDetails);

            SendAllGameDetailsToPlayersNotInGameAsync();

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

                RemoveConnectionFromAllMaps(Context.ConnectionId);

                SendGameDetailsToChannelGroupAsync(gameDetails, userGroupName);
                SendAllGameDetailsToPlayersNotInGameAsync();

                _gameService.UnAssignRoleToPlayerAsync(userGroupName, leavingPlayerDetails);
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
                gameDetails.IsAvailable = gameDetails.PlayersDetails.Count < MAX_GAME_COUNT;
                _groupNameToGameDetailsMappingTable[command.ChannelName] = gameDetails;

                SendAllGameDetailsToPlayersNotInGameAsync();

                SendGameDetailsToChannelGroupAsync(gameDetails,
                                                   command.ChannelName);

                //todo add user to discord channel
                if (!string.IsNullOrEmpty(newPlayerDetails.DiscordTag))
                {
                    _gameService.AssignRoleToPlayerAsync(command, newPlayerDetails);
                }
            }
        }

        #endregion
    }
}
