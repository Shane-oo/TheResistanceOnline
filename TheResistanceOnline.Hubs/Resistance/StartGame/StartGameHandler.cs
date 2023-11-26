using TheResistanceOnline.Common;
using TheResistanceOnline.Core.Errors;
using TheResistanceOnline.Core.NewCommandAndQueriesAndResultsPattern;
using TheResistanceOnline.Data;
using TheResistanceOnline.Data.Queries;
using TheResistanceOnline.Hubs.Common;

namespace TheResistanceOnline.Hubs.Resistance;

public class StartGameHandler: ICommandHandler<StartGameCommand, bool>
{
    #region Fields

    private readonly IDataContext _context;
    private static readonly SemaphoreLocker _locker = new SemaphoreLocker();

    #endregion

    #region Construction

    public StartGameHandler(IDataContext context)
    {
        _context = context;
    }

    #endregion

    #region Public Methods

    public async Task<Result<bool>> Handle(StartGameCommand command, CancellationToken cancellationToken)
    {
        if (command == null)
        {
            return Result.Failure<bool>(Error.NullValue);
        }

        return await _locker.LockAsync(async () =>
                                       {
                                           if (command.GameDetails.GameCommenced)
                                           {
                                               return Result.Failure<bool>(new Error("StartGame.GameCommenced", "Game Already Started"));
                                           }


                                           var user = await _context.Query<IUserByUserIdDbQuery>()
                                                                    .WithParams(command.UserId)
                                                                    .WithNoTracking()
                                                                    .ExecuteAsync(cancellationToken);

                                           var notFoundResult = NotFoundError.FailIfNull(user);
                                           if (notFoundResult.IsFailure)
                                           {
                                               return Result.Failure<bool>(notFoundResult.Error);
                                           }

                                           var connection = new ConnectionModel
                                                            {
                                                                ConnectionId = command.ConnectionId,
                                                                UserName = user.UserName
                                                            };
                                           command.GameDetails.Connections.Add(connection);
                                           command.GameDetails.BotsAllowed = command.BotsAllowed;
                                           command.GameDetails.InitialBotCount = command.Bots;
                                           command.GameDetails.GameType = command.Type;

                                           // return whether or not to start game is all expected connections are now connected
                                           return command.UserNames.All(command.GameDetails.Connections.Select(c => c.UserName).Contains) &&
                                                  command.UserNames.Count == command.GameDetails.Connections.Count;
                                       });
    }

    #endregion
}
