using Discord;
using Discord.WebSocket;
using TheResistanceOnline.BusinessLogic.Games.Commands;

namespace TheResistanceOnline.BusinessLogic.Games
{
    public interface IGameService
    {
        void CreateNewGameDiscordChatAsync(CreateGameCommand command);
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

        public async void CreateNewGameDiscordChatAsync(CreateGameCommand command)
        {
            await _discordSocketClient.LoginAsync(TokenType.Bot, "MTAxNTkyNTAxMjc1MTk5MDg1NA.GZ9H5M.PSRnP3LEhfP_DWFEp0cULEpf0ciDWgrq2HqCVQ");
            await _discordSocketClient.StartAsync();
            var newChannel = await  _discordSocketClient.GetChannelAsync(81889909113225237);
            Console.WriteLine(newChannel);
        }

        #endregion
    }
}
