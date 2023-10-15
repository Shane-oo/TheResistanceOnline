using MediatR;
using Microsoft.AspNetCore.SignalR;
using TheResistanceOnline.GamePlay;
using TheResistanceOnline.GamePlay.Common;
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

        command.GameDetails.GameModel.SetupGame(command.GameDetails.Connections
                                                       .Select(c => c.UserName)
                                                       .ToList(),
                                                command.GameDetails.InitialBotCount);

        foreach(var connection in command.GameDetails.Connections)
        {
            var playerDetails = command.GameDetails.GameModel.Players[connection.UserName];
            var commenceGameModel = new CommenceGameModel
                                    {
                                        Team = playerDetails.Team,
                                        TeamMates = playerDetails.Team == Team.Spy
                                                        ? command.GameDetails.GameModel.Players
                                                                 .Where(p => p.Value.Team == Team.Spy && p.Key != connection.UserName)
                                                                 .Select(p => p.Key)
                                                                 .ToList()
                                                        : null,
                                        MissionLeader = command.GameDetails.GameModel.MissionLeader,
                                        Phase = command.GameDetails.GameModel.Phase,
                                        Players = command.GameDetails.GameModel.PlayerNames
                                    };
            commenceGameModel.IsMissionLeader = commenceGameModel.MissionLeader == playerDetails.Name;
            await _resistanceHubContext.Clients.Client(connection.ConnectionId).CommenceGame(commenceGameModel);
        }

        return default;
    }

    #endregion
}
