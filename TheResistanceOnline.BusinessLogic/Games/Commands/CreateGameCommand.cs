using System.ComponentModel.DataAnnotations;
using TheResistanceOnline.BusinessLogic.Core.Commands;

namespace TheResistanceOnline.BusinessLogic.Games.Commands
{
    public class CreateGameCommand: CommandBase
    {
        #region Properties

        public bool CreateChatChannel { get; set; }

        public bool CreateVoiceChannel { get; set; }

        [Required]
        [MaxLength(50)]
        public string LobbyName { get; set; }

        public string userId { get; set; }

        #endregion
    }
}
