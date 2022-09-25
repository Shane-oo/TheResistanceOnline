using TheResistanceOnline.BusinessLogic.Core.Commands;

namespace TheResistanceOnline.BusinessLogic.DiscordServer.Commands
{
    public class CreateDiscordUserCommand: CommandBase
    {
        #region Properties

        public string AccessToken { get; set; }

        public string TokenType { get; set; }

        #endregion
    }
}
