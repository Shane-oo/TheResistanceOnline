using JetBrains.Annotations;
using TheResistanceOnline.BusinessLogic.Games.Models;
using TheResistanceOnline.Core.Requests.Commands;

namespace TheResistanceOnline.BusinessLogic.Games.Commands;

[UsedImplicitly]
public class StartGameCommand: CommandBase
{
    #region Properties

    public GameOptionsModel GameOptions { get; set; }

    #endregion
}
