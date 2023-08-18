using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using TheResistanceOnline.Core.Requests.Commands;

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
