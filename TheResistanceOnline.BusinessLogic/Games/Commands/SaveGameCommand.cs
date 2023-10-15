using TheResistanceOnline.BusinessLogic.Games.Models;
using TheResistanceOnline.Core.Requests.Commands;

namespace TheResistanceOnline.BusinessLogic.Games.Commands;

public class SaveGameCommand: CommandBase
{
    #region Properties

    public GameDetailsModel GameDetails { get; set; }

    #endregion
}

