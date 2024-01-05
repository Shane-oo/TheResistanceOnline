using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using TheResistanceOnline.Core.Errors;
using TheResistanceOnline.Core.Exceptions;
using TheResistanceOnline.Core.Exchange.Responses;
using TheResistanceOnline.Core.NewCommandAndQueriesAndResultsPattern;
using TheResistanceOnline.Data;
using TheResistanceOnline.Data.Queries;
using TheResistanceOnline.Hubs.Common;

namespace TheResistanceOnline.Hubs.Lobbies;

public class CreateLobbyHandler: ICommandHandler<CreateLobbyCommand, string>
{
    #region Fields

    private readonly IDataContext _context;
    private readonly IHubContext<LobbyHub, ILobbyHub> _lobbyHubContext;

    #endregion

    #region Construction

    public CreateLobbyHandler(IDataContext context, IHubContext<LobbyHub, ILobbyHub> lobbyHubContext)
    {
        _context = context;
        _lobbyHubContext = lobbyHubContext;
    }

    #endregion

    #region Public Methods

    public async Task<Result<string>> Handle(CreateLobbyCommand command, CancellationToken cancellationToken)
    {
        if (command == null)
        {
            return Result.Failure<string>(Error.NullValue);
        }

        if (command.GroupNamesToLobby.ContainsKey(command.Id))
        {
            return Result.Failure<string>(new Error("CreateLobby.AlreadyExists", $"{command.Id} Already Exists"));
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

        return lobbyDetails.Id;
    }

    #endregion
}
