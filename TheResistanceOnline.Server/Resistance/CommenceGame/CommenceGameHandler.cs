using Microsoft.AspNetCore.SignalR;
using TheResistanceOnline.Common;
using TheResistanceOnline.Core.Errors;
using TheResistanceOnline.Core.Exchange.Requests;
using TheResistanceOnline.Core.Exchange.Responses;
using TheResistanceOnline.GamePlay;
using TheResistanceOnline.GamePlay.Common;
using TheResistanceOnline.GamePlay.GameModels;

namespace TheResistanceOnline.Server.Resistance;

public class CommenceGameHandler: ICommandHandler<CommenceGameCommand>
{
    #region Fields

    private readonly IHubContext<ResistanceHub, IResistanceHub> _resistanceHubContext;
    private static readonly SemaphoreLocker _locker = new SemaphoreLocker();

    #endregion

    #region Construction

    public CommenceGameHandler(IHubContext<ResistanceHub, IResistanceHub> resistanceHubContext)
    {
        _resistanceHubContext = resistanceHubContext;
    }

    #endregion

    #region Public Methods

    public async Task<Result> Handle(CommenceGameCommand command, CancellationToken cancellationToken)
    {
        if (command == null)
        {
            return Result.Failure<string>(Error.NullValue);
        }

        return await _locker.LockAsync(async () =>
                                       {
                                           // in case of case where commenceGameCommand is sent twice 
                                           // immediately commence game and hopefully in the other thread game has been commenced
                                           if (command.GameDetails.GameCommenced) return Result.Success();

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

                                           return Result.Success();
                                       });
    }

    #endregion
}
