using JetBrains.Annotations;

namespace TheResistanceOnline.BusinessLogic.Games.Models;

public class GameDetailsResponse
{
    #region Properties

    [CanBeNull]
    public string ErrorMessage { get; set; }

    public bool ErrorOccured { get; set; }

    public GameDetailsModel GameDetails { get; set; }

    #endregion
}
