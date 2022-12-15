using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Discord;
using Discord.WebSocket;
using Flurl.Http;
using TheResistanceOnline.BusinessLogic.DiscordServer.Commands;
using TheResistanceOnline.BusinessLogic.DiscordServer.Models;
using TheResistanceOnline.BusinessLogic.Settings;
using TheResistanceOnline.BusinessLogic.Users.DbQueries;
using TheResistanceOnline.Data;
using TheResistanceOnline.Data.DiscordServer;

namespace TheResistanceOnline.BusinessLogic.DiscordServer
{
    public interface IDiscordServerService
    {
        void AddRoleToUserAsync(string roleName, string discordTag);

        Task CreateDiscordUserAsync([NotNull] CreateDiscordUserCommand command);

        Task<bool> DiscordUserInServerAsync(DiscordUser discordUser);

        void RemoveRoleFromUserAsync(string roleName, string discordTag);
    }

    public class DiscordServerService: IDiscordServerService
    {
        #region Constants

        private const long SERVER_GUILD_ID = 1010777112703144017;

        #endregion

        #region Fields

        private readonly Dictionary<string, string> _channelNameToRoleMap = new Dictionary<string, string>
                                                                            {
                                                                                {
                                                                                    "game-1",
                                                                                    "Join Game-1"
                                                                                },
                                                                                {
                                                                                    "game-2",
                                                                                    "Join Game-2"
                                                                                },
                                                                                {
                                                                                    "game-3",
                                                                                    "Join Game-3"
                                                                                },
                                                                                {
                                                                                    "game-4",
                                                                                    "Join Game-4"
                                                                                },
                                                                                {
                                                                                    "game-5",
                                                                                    "Join Game-5"
                                                                                },
                                                                                {
                                                                                    "game-6",
                                                                                    "Join Game-6"
                                                                                },
                                                                                {
                                                                                    "game-7",
                                                                                    "Join Game-7"
                                                                                },
                                                                                {
                                                                                    "game-8",
                                                                                    "Join Game-8"
                                                                                },
                                                                                {
                                                                                    "game-9",
                                                                                    "Join Game-9"
                                                                                },
                                                                                {
                                                                                    "game-10",
                                                                                    "Join Game-10"
                                                                                }
                                                                            };

        private readonly IDataContext _context;

        private readonly DiscordSocketClient _discordSocketClient;
        private readonly IMapper _mapper;
        private readonly ISettingsService _settings;

        #endregion

        #region Construction

        public DiscordServerService(IDataContext context, ISettingsService settings, DiscordSocketClient discordSocketClient, IMapper mapper)
        {
            _context = context;

            _discordSocketClient = discordSocketClient;
            _mapper = mapper;
            _settings = settings;
        }

        #endregion

        #region Private Methods

        private async Task CheckBotIsConnectedAsync()
        {
            if (_discordSocketClient.ConnectionState != ConnectionState.Connected)
            {
                await ConnectBotAsync();
            }
        }

        private async Task ConnectBotAsync()
        {
            if (_discordSocketClient.ConnectionState != ConnectionState.Connecting)
            {
                await _discordSocketClient.LoginAsync(TokenType.Bot, _settings.GetAppSettings().DiscordLoginToken);
                await _discordSocketClient.StartAsync();
            }

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

            var guild = _discordSocketClient.Guilds.FirstOrDefault(g => g.Id == SERVER_GUILD_ID);
            if (guild != null)
            {
                // wait for guild channels, roles and users fetched
                while(guild.Channels.Count == 0)
                {
                    await Task.Delay(25);
                }

                while(guild.Roles.Count == 0)
                {
                    await Task.Delay(25);
                }

                while(guild.Users.Count <= 1)
                {
                    await Task.Delay(25);
                }
            }
        }

        #endregion

        #region Public Methods

        public async void AddRoleToUserAsync(string channelName, string discordTag)
        {
            await CheckBotIsConnectedAsync();

            var roleName = _channelNameToRoleMap[channelName];
            // Only One Guild/Server so get First
            var guild = _discordSocketClient.Guilds.FirstOrDefault(g => g.Id == SERVER_GUILD_ID);
            if (guild != null)
            {
                var role = guild.Roles.FirstOrDefault(r => r.Name == roleName);
                var user = guild.Users.FirstOrDefault(x => x.Username + "#" + x.Discriminator == discordTag);
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
            user.UserSetting.UserWantsToUseDiscord = true;
            user.UserSetting.UserWantsToUseDiscordRecord = DateTimeOffset.UtcNow;

            await _context.SaveChangesAsync(command.CancellationToken);
        }

        // returns false if user not in server
        // not used decided to scrap idea
        public async Task<bool> DiscordUserInServerAsync(DiscordUser discordUser)
        {
            await CheckBotIsConnectedAsync();

            var guild = _discordSocketClient.Guilds.FirstOrDefault(g => g.Id == SERVER_GUILD_ID);
            var user = guild?.Users.FirstOrDefault(x => x.Username + "#" + x.Discriminator == discordUser.DiscordTag);
            return user != null;
        }

        public async void RemoveRoleFromUserAsync(string channelName, string discordTag)
        {
            await CheckBotIsConnectedAsync();
            var roleName = _channelNameToRoleMap[channelName];
            var guild = _discordSocketClient.Guilds.FirstOrDefault(g => g.Id == SERVER_GUILD_ID);
            if (guild != null)
            {
                var role = guild.Roles.FirstOrDefault(r => r.Name == roleName);
                var user = guild.Users.FirstOrDefault(x => x.Username + "#" + x.Discriminator == discordTag);
                if (user != null && role != null)
                {
                    await user.RemoveRoleAsync(role);
                }
            }
        }

        #endregion
    }
}
