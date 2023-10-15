using MediatR;
using TheResistanceOnline.Core.Requests.Commands;
using TheResistanceOnline.Games;
using TheResistanceOnline.Hubs.Resistance.Common;

namespace TheResistanceOnline.Hubs.Resistance.CommenceGame;

public class CommenceGameCommand: CommandBase<Unit>

{
    #region Properties

    public GameDetails GameDetails { get; set; }

    public string LobbyId { get; set; }

    #endregion
}
