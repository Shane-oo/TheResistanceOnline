using FluentValidation;
using MediatR;
using TheResistanceOnline.Core.Exceptions;
using TheResistanceOnline.Data;
using TheResistanceOnline.Data.Entities.UserEntities;
using TheResistanceOnline.Data.Queries.UserQueries;
using TheResistanceOnline.Hubs.Common;

namespace TheResistanceOnline.Hubs.Resistance.StartGame;

public class StartGameHandler: IRequestHandler<StartGameCommand, bool>
{
    #region Fields

    private readonly IDataContext _context;

    private readonly IValidator<StartGameCommand> _validator;

    #endregion

    #region Construction

    public StartGameHandler(IDataContext context, IValidator<StartGameCommand> validator)
    {
        _context = context;
        _validator = validator;
    }

    #endregion

    #region Public Methods

    public async Task<bool> Handle(StartGameCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid) throw new DomainException(validationResult.Errors.First().ErrorMessage);

        if (command.GameDetails.GameCommenced)
        {
            throw new DomainException("Game has already started");
        }


        var user = await _context.Query<IUserByUserIdDbQuery>()
                                 .WithParams(command.UserId)
                                 .WithNoTracking()
                                 .ExecuteAsync(cancellationToken);

        NotFoundException.ThrowIfNull(user);

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
    }

    #endregion
}
