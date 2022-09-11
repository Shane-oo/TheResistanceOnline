using System.Diagnostics.CodeAnalysis;
using TheResistanceOnline.BusinessLogic.DiscordServer;
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

        private readonly IDiscordServerService _discordServerService;

        #endregion

        #region Construction

        public GameService(IDiscordServerService discordServerService)
        {
            _discordServerService = discordServerService;
        }

        #endregion

        #region Public Methods

        public async void AssignRoleToPlayerAsync(CreateGameCommand command, UserDetailsModel userDetails)
        {
            await _discordServerService.AddRoleToUserAsync("Join Game-1","game-1");
        }

        #endregion
    }
}
