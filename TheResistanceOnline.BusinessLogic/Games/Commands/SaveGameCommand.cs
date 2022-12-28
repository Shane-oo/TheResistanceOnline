using TheResistanceOnline.BusinessLogic.Core.Commands;
using TheResistanceOnline.BusinessLogic.Games.Models;

namespace TheResistanceOnline.BusinessLogic.Games.Commands;

public class SaveGameCommand: CommandBase
{
    #region Properties

    public GameDetailsModel GameDetails { get; set; }

    #endregion
}

