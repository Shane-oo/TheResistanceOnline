using JetBrains.Annotations;
using TheResistanceOnline.BusinessLogic.Core.Commands;
using TheResistanceOnline.BusinessLogic.Games.Models;

namespace TheResistanceOnline.BusinessLogic.Games.Commands;

[UsedImplicitly]
public class StartGameCommand: CommandBase
{
    #region Properties

    public GameOptionsModel GameOptions { get; set; }

    #endregion
}
