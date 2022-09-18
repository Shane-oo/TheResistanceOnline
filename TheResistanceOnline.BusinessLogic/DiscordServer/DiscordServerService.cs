using Discord;
using Discord.WebSocket;

namespace TheResistanceOnline.BusinessLogic.DiscordServer
{
    public interface IDiscordServerService
    {
        Task AddRoleToUserAsync(string roleName, string channelName);
    }

    public class DiscordServerService: IDiscordServerService
    {
        #region Fields

        private readonly DiscordSocketClient _discordSocketClient;

        #endregion

        #region Construction

        public DiscordServerService(DiscordSocketClient discordSocketClient)
        {
            _discordSocketClient = discordSocketClient;
        }

        #endregion

        #region Private Methods

        private async Task ConnectBotAsync()
        {
            //todo make environment variable
            await _discordSocketClient.LoginAsync(TokenType.Bot, "MTAxNTkyNTAxMjc1MTk5MDg1NA.GZ9H5M.PSRnP3LEhfP_DWFEp0cULEpf0ciDWgrq2HqCVQ");
            await _discordSocketClient.StartAsync();
            // wait for bot to be connected
            while(_discordSocketClient.ConnectionState != ConnectionState.Connected)
            {
                await Task.Delay(25);
            }

            // wait for guild to be fetched
            while(_discordSocketClient.Guilds.Count == 0)
            {
                await Task.Delay(25);
            }

            var guild = _discordSocketClient.Guilds.First();
            // wait for guild channels, roles and users fetched
            while(guild.Channels.Count == 0 && guild.Roles.Count == 0 && guild.Users.Count <= 1)
            {
                await Task.Delay(25);
            }
            
        }

        #endregion

        #region Public Methods

        public async Task AddRoleToUserAsync(string roleName, string channelName)
        {
            if (_discordSocketClient.ConnectionState != ConnectionState.Connected)
            {
                await ConnectBotAsync();
            }

            // Only One Guild/Server so get First
            var guild = _discordSocketClient.Guilds.FirstOrDefault();
            if (guild != null)
            {
                var channel = guild.Channels.FirstOrDefault(c => c.Name == channelName);
                var role = guild.Roles.FirstOrDefault(r => r.Name == roleName);
                var user = guild.Users.FirstOrDefault(x => x.Discriminator =="9963" && x.Username== "shane");
                if (user != null && role != null)
                {
                    await user.AddRoleAsync(role);
                }
            }
        }

        #endregion
    }
}
