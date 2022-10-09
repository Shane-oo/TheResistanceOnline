using System.Diagnostics.CodeAnalysis;
using TheResistanceOnline.BusinessLogic.DiscordServer;
using TheResistanceOnline.BusinessLogic.Games.Commands;
using TheResistanceOnline.BusinessLogic.Games.Models;

namespace TheResistanceOnline.BusinessLogic.Games
{
    public interface IGameService
    {
 
    }

    public class GameService: IGameService
    {
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

        private readonly IDiscordServerService _discordServerService;

        #endregion

        #region Construction

        public GameService(IDiscordServerService discordServerService)
        {
            _discordServerService = discordServerService;
        }

        #endregion

        #region Public Methods

    
        #endregion
    }
}
