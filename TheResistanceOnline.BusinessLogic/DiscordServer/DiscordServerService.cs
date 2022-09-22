using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Discord;
using Discord.WebSocket;
using Flurl.Http;
using TheResistanceOnline.BusinessLogic.DiscordServer.Commands;
using TheResistanceOnline.BusinessLogic.DiscordServer.Models;
using TheResistanceOnline.BusinessLogic.Users.DbQueries;
using TheResistanceOnline.Data;
using TheResistanceOnline.Data.DiscordServer;

namespace TheResistanceOnline.BusinessLogic.DiscordServer
{
    public interface IDiscordServerService
    {
        Task AddRoleToUserAsync(string roleName, string channelName);

        Task CreateDiscordUserAsync([NotNull] CreateDiscordUserCommand command);
    }

    public class DiscordServerService: IDiscordServerService
    {
        #region Fields

        private readonly IDataContext _context;

        private readonly DiscordSocketClient _discordSocketClient;
        private readonly IMapper _mapper;

        #endregion

        #region Construction

        public DiscordServerService(IDataContext context, DiscordSocketClient discordSocketClient, IMapper mapper)
        {
            _context = context;

            _discordSocketClient = discordSocketClient;
            _mapper = mapper;
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
                var user = guild.Users.FirstOrDefault(x => x.Discriminator == "9963" && x.Username == "shane");
                if (user != null && role != null)
                {
                    await user.AddRoleAsync(role);
                }
            }
        }

        public async Task CreateDiscordUserAsync(CreateDiscordUserCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            var user = await _context.Query<IUserDbQuery>().WithParams(command.UserId)
                                     .ExecuteAsync(command.CancellationToken);

            const string REQUEST_URI = "https://discord.com/api/users/@me";
            var discordUserResponse = await REQUEST_URI.WithOAuthBearerToken(command.AccessToken).GetJsonAsync<DiscordUserResponseModel>();
            var discordUser = _mapper.Map<DiscordUser>(discordUserResponse);
            user.DiscordUser = discordUser;

            await _context.SaveChangesAsync(command.CancellationToken);
        }

        #endregion
    }
}
