using System.Diagnostics.CodeAnalysis;
using Discord;
using Discord.WebSocket;
using TheResistanceOnline.BusinessLogic.Games.Commands;
using TheResistanceOnline.BusinessLogic.Users.Models;

namespace TheResistanceOnline.BusinessLogic.Games
{
    public interface IGameService
    {
        void AssignRoleToPlayerAsync([NotNull] CreateGameCommand command, [NotNull] UserDetailsModel userDetails);
    }

    public class GameService: IGameService
    {
        #region Fields

        private readonly DiscordSocketClient _discordSocketClient;

        #endregion

        #region Construction

        public GameService(DiscordSocketClient discordSocketClient)
        {
            _discordSocketClient = discordSocketClient;
        }

        #endregion

        #region Public Methods

        public async void AssignRoleToPlayerAsync(CreateGameCommand command, UserDetailsModel userDetails)
        {
            await _discordSocketClient.LoginAsync(TokenType.Bot, "MTAxNTkyNTAxMjc1MTk5MDg1NA.GZ9H5M.PSRnP3LEhfP_DWFEp0cULEpf0ciDWgrq2HqCVQ");
            await _discordSocketClient.StartAsync();
            // need to wait for connected
            while(_discordSocketClient.ConnectionState != ConnectionState.Connected)
            {
                 Console.WriteLine(_discordSocketClient.ConnectionState);
            }
            // there exists guilds , count =1
            Console.WriteLine(userDetails);
            var privateChannels = _discordSocketClient.PrivateChannels;
            foreach(var VARIABLE in privateChannels)
            {
                Console.WriteLine(VARIABLE);
            }
            
        }

        #endregion
    }
}
