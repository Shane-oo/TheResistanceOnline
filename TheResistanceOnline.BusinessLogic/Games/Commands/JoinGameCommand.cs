using System.ComponentModel.DataAnnotations;
using TheResistanceOnline.BusinessLogic.Core.Commands;

namespace TheResistanceOnline.BusinessLogic.Games.Commands
{
    public class JoinGameCommand: CommandBase
    {
        #region Properties

        [Required]
        [MaxLength(50)]
        public string LobbyName { get; set; }

        #endregion
    }
}
