using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using TheResistanceOnline.Core.Exceptions;
using TheResistanceOnline.Data;
using TheResistanceOnline.Data.Entities.UserEntities;
using TheResistanceOnline.Data.Queries.UserQueries;
using TheResistanceOnline.Games.Lobbies;
using TheResistanceOnline.Hubs.Common;
using TheResistanceOnline.Hubs.Lobbies.Common;

namespace TheResistanceOnline.Hubs.Lobbies.CreateLobby;

public class CreateLobbyHandler: IRequestHandler<CreateLobbyCommand, LobbyDetailsModel>
{
    #region Fields

    private readonly IDataContext _context;
    private readonly IHubContext<LobbyHub, ILobbyHub> _lobbyHubContext;
    private readonly IValidator<CreateLobbyCommand> _validator;

    #endregion

    #region Construction

    public CreateLobbyHandler(IDataContext context, IHubContext<LobbyHub, ILobbyHub> lobbyHubContext, IValidator<CreateLobbyCommand> validator)
    {
        _context = context;
        _lobbyHubContext = lobbyHubContext;
        _validator = validator;
    }

    #endregion

    #region Public Methods

    public async Task<LobbyDetailsModel> Handle(CreateLobbyCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        UnauthorizedException.ThrowIfUserIsNotAllowedAccess(command, Roles.User);

        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid) throw new DomainException(validationResult.Errors.First().ErrorMessage);

        if (command.GroupNamesToLobby.ContainsKey(command.Id))
        {
            throw new DomainException("Lobby Already Exists With That Id");
        }

        var user = await _context.Query<IUserByUserIdDbQuery>()
                                 .WithParams(command.UserId)
                                 .WithNoTracking()
                                 .ExecuteAsync(cancellationToken);

        NotFoundException.ThrowIfNull(user);

        await _lobbyHubContext.Groups.AddToGroupAsync(command.ConnectionId, command.Id, cancellationToken);

        var lobbyDetails = new LobbyDetailsModel
                           {
                               HostConnectionId = command.ConnectionId,
                               Connections = new List<ConnectionModel>
                                             {
                                                 new()
                                                 {
                                                     ConnectionId = command.ConnectionId,
                                                     UserName = user.UserName
                                                 }
                                             },
                               Id = command.Id,
                               IsPrivate = command.IsPrivate,
                               MaxPlayers = command.MaxPlayers,
                               FillWithBots = command.FillWithBots,
                               TimeCreated = DateTime.UtcNow
                           };

        command.GroupNamesToLobby[command.Id] = lobbyDetails;
        if (!lobbyDetails.IsPrivate)
        {
            var allConnectionsInLobbies = command.GroupNamesToLobby.Values
                                                 .SelectMany(c => c.Connections)
                                                 .Select(c => c.ConnectionId)
                                                 .ToList();

            await _lobbyHubContext.Clients.AllExcept(allConnectionsInLobbies).NewPublicLobby(lobbyDetails);
        }

        return lobbyDetails;
    }

    #endregion
}
