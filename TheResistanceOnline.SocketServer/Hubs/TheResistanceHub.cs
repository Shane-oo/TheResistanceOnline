using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using TheResistanceOnline.BusinessLogic.Core.Queries;
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

        private static readonly Dictionary<string, string> _connectionIdToGroupMappingTable = new Dictionary<string, string>();
        private static readonly Dictionary<string, UserDetailsModel> _connectionIdToUserMappingTable = new Dictionary<string, UserDetailsModel>();
        private static readonly Dictionary<string, GameDetailsModel> _groupNameToGameDetailsMappingTable = new Dictionary<string, GameDetailsModel>();

        private readonly IUserService _userService;

        #endregion

        #region Construction

        public TheResistanceHub(IUserService userService)
        {
            _userService = userService;
        }

        #endregion

        #region Public Methods

        public async Task BroadcastMessageData(string message)
        {
            await Clients.All.SendAsync("broadcastmessagedata", message);
        }


        public async Task CreateGame(CreateGameCommand command)
        {
            if (_groupNameToGameDetailsMappingTable.Count == MAX_GAME_COUNT)
            {
                await Clients.Client(Context.ConnectionId).SendAsync("tooManyGames", "There Exists Too Many Games Currently Playing. " +
                                                                                     "Please Wait Until Games Are Finished Before Creating A New Game");
                return;
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, command.LobbyName);
            _connectionIdToGroupMappingTable.Add(Context.ConnectionId, command.LobbyName);
            var playerDetails = new PlayerDetailsModel
                                {
                                    UserName = _connectionIdToUserMappingTable[Context.ConnectionId].UserName,
                                    ProfilePictureName = _connectionIdToUserMappingTable[Context.ConnectionId].ProfilePicture?.Name
                                };

            var newGame = new GameDetailsModel
                          {
                              LobbyName = command.LobbyName,
                              UserInGame = true,
                              PlayersDetails = new List<PlayerDetailsModel>
                                               {
                                                   playerDetails
                                               }
                          };
            _groupNameToGameDetailsMappingTable.Add(command.LobbyName, newGame);

            await Clients.Group(command.LobbyName).SendAsync("userCreatedGame", newGame);
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
            try
            {
                var groupConnectionWasIn = _connectionIdToGroupMappingTable[Context.ConnectionId];
                var userThatLeft = _connectionIdToUserMappingTable[Context.ConnectionId].UserName;
                Clients.Group(groupConnectionWasIn).SendAsync("userLeftLobby", $"{userThatLeft}");
                _connectionIdToUserMappingTable.Remove(Context.ConnectionId);
                _connectionIdToGroupMappingTable.Remove(Context.ConnectionId);
            }
            catch(Exception ex)
            {
                // key does not exist in dictionary's all goods
            }

            return base.OnDisconnectedAsync(exception);
        }

        #endregion
    }
}
