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

        private static readonly Dictionary<string, string> _connectionIdToGroupNameMappingTable = new Dictionary<string, string>();
        
        private static readonly Dictionary<string, Guid> _connectionIdToPlayerId = new Dictionary<string, Guid>();
        private static readonly Dictionary<string, UserDetailsModel> _connectionIdToUserMappingTable = new Dictionary<string, UserDetailsModel>();
        private static readonly Dictionary<string, GameDetailsModel> _groupNameToGameDetailsMappingTable = new Dictionary<string, GameDetailsModel>();

        private readonly IUserService _userService;
        private readonly IGameService _gameService;
        #endregion

        #region Construction

        public TheResistanceHub(IUserService userService,IGameService gameService)
        {
            _userService = userService;
            _gameService = gameService;
        }

        #endregion

        #region Private Methods

        private PlayerDetailsModel? CreateNewPlayer()
        {
            var playerDetails = new PlayerDetailsModel
                                {
                                    PlayerId = Guid.NewGuid(),
                                    UserName = _connectionIdToUserMappingTable[Context.ConnectionId].UserName,
                                    ProfilePictureId = _connectionIdToUserMappingTable[Context.ConnectionId].ProfilePicture?.Id
                                };
            if (string.IsNullOrEmpty(playerDetails.UserName))
            {
                return null;
            }

            _connectionIdToPlayerId.Add(Context.ConnectionId, playerDetails.PlayerId);
            return playerDetails;
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
                _gameService.CreateNewGameDiscordChatAsync(command);
                
                // todo denote the host
                var newGame = new GameDetailsModel
                              {
                                  LobbyName = command.LobbyName,
                                  PlayersDetails = new List<PlayerDetailsModel>
                                                   {
                                                       playerDetails
                                                   }
                              };

                _groupNameToGameDetailsMappingTable.Add(command.LobbyName, newGame);

                await Clients.Group(command.LobbyName).SendAsync("userCreatedGame", newGame);
            }
            else
            {
                //error
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
            var userDetails = await _userService.GetUserByEmailOrNameAsync(new ByIdAndNameQuery
                                                                           {
                                                                               Name = Context.User?.Identity?.Name
                                                                           });
            _connectionIdToUserMappingTable.Add(Context.ConnectionId, userDetails);

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

            _connectionIdToUserMappingTable.Remove(Context.ConnectionId);

            return base.OnDisconnectedAsync(exception);
        }

        #endregion
    }
}