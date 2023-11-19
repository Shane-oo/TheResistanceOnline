using MediatR;
using TheResistanceOnline.Core.Requests.Commands;

namespace TheResistanceOnline.Hubs.Resistance;

public class CommenceGameCommand: CommandBase<Unit>

{
    #region Properties

    public GameDetails GameDetails { get; set; }

    public string LobbyId { get; set; }

    #endregion
}
