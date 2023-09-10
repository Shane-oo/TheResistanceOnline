using MediatR;
using Microsoft.AspNetCore.SignalR;
using TheResistanceOnline.Core.Exceptions;
using TheResistanceOnline.Data;
using TheResistanceOnline.Data.Entities.UserEntities;
using TheResistanceOnline.Data.Queries.UserQueries;
using TheResistanceOnline.Games.Lobbies.Common;

namespace TheResistanceOnline.Games.Lobbies.JoinLobby;

public class JoinLobbyHandler: IRequestHandler<JoinLobbyCommand, LobbyDetailsModel>
{
    #region Fields

    private readonly IDataContext _context;
    private readonly IHubContext<LobbyHub, ILobbyHub> _lobbyHubContext;

    #endregion

    #region Construction

    public JoinLobbyHandler(IDataContext context, IHubContext<LobbyHub, ILobbyHub> lobbyHubContext)
    {
        _context = context;
        _lobbyHubContext = lobbyHubContext;
    }

    #endregion

    #region Public Methods

    public async Task<LobbyDetailsModel> Handle(JoinLobbyCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        UnauthorizedException.ThrowIfUserIsNotAllowedAccess(command, Roles.User);

        if (!command.GroupNamesToLobby.TryGetValue(command.LobbyId, out var lobbyDetails))
        {
            throw new DomainException("Lobby With That Id Was Not Found");
        }

        if (lobbyDetails.Connections.Count == 10)
        {
            throw new DomainException("Lobby Is Full");
        }

        var user = await _context.Query<IUserByUserIdDbQuery>()
                                 .WithParams(command.UserId)
                                 .WithNoTracking()
                                 .ExecuteAsync(cancellationToken);

        NotFoundException.ThrowIfNull(user);

        if (lobbyDetails.Connections.Any(c => c.UserName == user.UserName))
        {
            throw new DomainException("You're Already In This Lobby");
        }

        await _lobbyHubContext.Groups.AddToGroupAsync(command.ConnectionId, lobbyDetails.Id, cancellationToken);

        var connection = new ConnectionModel
                         {
                             ConnectionId = command.ConnectionId,
                             UserName = user.UserName
                         };
        lobbyDetails.Connections.Add(connection);

        await _lobbyHubContext.Clients.Group(lobbyDetails.Id).NewConnectionInLobby(connection);

        if (!lobbyDetails.IsPrivate)
        {
            var allConnectionsInLobbies = command.GroupNamesToLobby.Values
                                                 .SelectMany(c => c.Connections)
                                                 .Select(c => c.ConnectionId)
                                                 .ToList();
            await _lobbyHubContext.Clients.AllExcept(allConnectionsInLobbies).UpdatePublicLobby(lobbyDetails);
        }

        return lobbyDetails;
    }

    #endregion
}