using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using TheResistanceOnline.BusinessLogic.Core.Commands;

namespace TheResistanceOnline.BusinessLogic.Games.Commands
{
    [UsedImplicitly]
    public class CreateGameCommand: CommandBase
    {
        #region Properties
        
        [Required]
        [NotNull]
        public string LobbyName { get; set; }

        

        #endregion
    }
}
