using Microsoft.AspNetCore.SignalR;
using TheResistanceOnline.Core.Errors;
using TheResistanceOnline.Core.Exchange.Responses;
using TheResistanceOnline.Core.NewCommandAndQueriesAndResultsPattern;
using TheResistanceOnline.Data;
using TheResistanceOnline.Data.Queries;
using TheResistanceOnline.Hubs.Common;

namespace TheResistanceOnline.Hubs.Lobbies;

public class JoinLobbyHandler: ICommandHandler<JoinLobbyCommand, string>
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

    public async Task<Result<string>> Handle(JoinLobbyCommand command, CancellationToken cancellationToken)
    {
        if (command == null)
        {
            return Result.Failure<string>(Error.NullValue);
        }

        if (!command.GroupNamesToLobby.TryGetValue(command.LobbyId, out var lobbyDetails))
        {
            return Result.Failure<string>(NotFoundError.NotFound(command.LobbyId));
        }

        if (lobbyDetails.Connections.Count == lobbyDetails.MaxPlayers)
        {
            return Result.Failure<string>(new Error("JoinLobby.Full", $"{command.LobbyId} Is Full"));
        }

        var user = await _context.Query<IUserByUserIdDbQuery>()
                                 .WithParams(command.UserId)
                                 .WithNoTracking()
                                 .ExecuteAsync(cancellationToken);

        var notFoundResult = NotFoundError.FailIfNull(user);
        if (notFoundResult.IsFailure)
        {
            return Result.Failure<string>(notFoundResult.Error);
        }

        if (lobbyDetails.Connections.Any(c => c.UserName == user.UserName))
        {
            return Result.Failure<string>(new Error("JoinLobby.AlreadyJoined", $"You Already Joined {command.LobbyId}"));
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

        return lobbyDetails.Id;
    }

    #endregion
}
