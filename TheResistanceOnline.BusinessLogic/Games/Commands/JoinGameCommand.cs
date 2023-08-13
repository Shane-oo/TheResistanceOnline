using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using TheResistanceOnline.Core.Commands;

namespace TheResistanceOnline.BusinessLogic.Games.Commands
{
    [UsedImplicitly]
    public class JoinGameCommand: CommandBase
    {
        #region Properties

        [Required]
        public string ChannelName { get; set; }

        #endregion
    }
}
