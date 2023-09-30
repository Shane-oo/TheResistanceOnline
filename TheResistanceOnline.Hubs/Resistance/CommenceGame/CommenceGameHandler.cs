using MediatR;
using Microsoft.AspNetCore.SignalR;
using TheResistanceOnline.GamePlay;
using TheResistanceOnline.GamePlay.GameModels;

namespace TheResistanceOnline.Hubs.Resistance.CommenceGame;

public class CommenceGameHandler: IRequestHandler<CommenceGameCommand, Unit>
{
    #region Fields

    private readonly IHubContext<ResistanceHub, IResistanceHub> _resistanceHubContext;

    #endregion

    #region Construction

    public CommenceGameHandler(IHubContext<ResistanceHub, IResistanceHub> resistanceHubContext)
    {
        _resistanceHubContext = resistanceHubContext;
    }

    #endregion

    #region Public Methods

    public async Task<Unit> Handle(CommenceGameCommand command, CancellationToken cancellationToken)
    {
        // in case of case where commenceGameCommand is sent twice 
        // immediately commence game and hopefully in the other thread game has been commenced
        if (command.GameDetails.GameCommenced) return default;

        command.GameDetails.GameCommenced = true;

        if (command.GameDetails.GameType == GameType.ResistanceClassic)
        {
            command.GameDetails.GameModel = new ResistanceClassicGameModel();
        }

        command.GameDetails.GameModel.AssignTeams(command.GameDetails.Connections
                                                         .Select(c => c.UserName)
                                                         .ToList(),
                                                  command.GameDetails.InitialBotCount);

        Console.WriteLine(command.GameDetails.GameModel);

        foreach(var player in command.GameDetails.GameModel.Players)
        {
            player.Vote();
        }

        await _resistanceHubContext.Clients.Group(command.LobbyId).CommenceGame(); // todo listen for this on client side
        return default;
    }

    #endregion
}
