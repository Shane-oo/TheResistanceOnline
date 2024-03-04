using Microsoft.AspNetCore.SignalR;
using TheResistanceOnline.Common;
using TheResistanceOnline.Core.Errors;
using TheResistanceOnline.Core.Exchange.Requests;
using TheResistanceOnline.Core.Exchange.Responses;
using TheResistanceOnline.GamePlay;
using TheResistanceOnline.GamePlay.Common;
using TheResistanceOnline.GamePlay.GameModels;
using TheResistanceOnline.Server.Resistance.CommenceGame;

namespace TheResistanceOnline.Server.Resistance;

public class CommenceGameHandler: ICommandHandler<CommenceGameCommand>
{
    #region Fields

    private static readonly SemaphoreLocker _locker = new();

    private readonly IHubContext<ResistanceHub, IResistanceHub> _resistanceHubContext;

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
                                           var gameDetails = command.GameDetails;
                                           // in case of case where commenceGameCommand is sent twice 
                                           // immediately commence game and hopefully in the other thread game has been commenced
                                           if (gameDetails.GameCommenced) return Result.Success();

                                           gameDetails.GameCommenced = true;

                                           if (gameDetails.GameType == GameType.ResistanceClassic)
                                           {
                                               gameDetails.GameModel = new ResistanceClassicGameModel();
                                           }

                                           var gameModel = gameDetails.GameModel;

                                           gameModel.SetupGame(gameDetails.Connections
                                                                          .Select(c => c.UserName)
                                                                          .ToList(),
                                                               gameDetails.InitialBotCount);

                                           foreach(var connection in gameDetails.Connections)
                                           {
                                               var playerDetails = gameModel.Players[connection.UserName];
                                               var commenceGameModel = new CommenceGameModel
                                                                       {
                                                                           Team = playerDetails.Team,
                                                                           TeamMates = playerDetails.Team == Team.Spy
                                                                                           ? gameModel.Players
                                                                                                      .Where(p => p.Value.Team == Team.Spy && p.Key != connection.UserName)
                                                                                                      .Select(p => p.Key)
                                                                                                      .ToList()
                                                                                           : null,
                                                                           MissionLeader = gameModel.MissionLeader,
                                                                           Phase = gameModel.Phase,
                                                                           Players = gameModel.PlayerNames
                                                                       };
                                               await _resistanceHubContext.Clients.Client(connection.ConnectionId).CommenceGame(commenceGameModel);
                                           }

                                           switch(gameModel.Phase)
                                           {
                                               case Phase.MissionBuild:
                                                   var missionLeaderConnection = gameDetails.Connections.FirstOrDefault(p => p.UserName == gameModel.MissionLeader);
                                                   if (missionLeaderConnection != null)
                                                   {
                                                       await _resistanceHubContext.Clients.Client(missionLeaderConnection.ConnectionId).StartMissionBuildPhase();
                                                   }

                                                   break;
                                               case Phase.Vote:
                                                   var missionTeam = gameModel.GetMissionTeam();
                                                   await _resistanceHubContext.Clients.Group(command.LobbyId).VoteForMissionTeam(missionTeam);

                                                   foreach(var bot in gameModel.Bots)
                                                   {
                                                       await _resistanceHubContext.Clients.Group(command.LobbyId).PlayerVoted(bot.Name);
                                                   }

                                                   break;
                                               case Phase.Mission:
                                               case Phase.MissionResults:
                                               default:
                                                   throw new ArgumentOutOfRangeException();
                                           }

                                           return Result.Success();
                                       });
    }

    #endregion
}
