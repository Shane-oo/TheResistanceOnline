using System.Collections.Concurrent;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using TheResistanceOnline.Data;
using TheResistanceOnline.Data.Queries.UserQueries;
using TheResistanceOnline.Hubs.Common;
using TheResistanceOnline.Hubs.Streams.Common;

namespace TheResistanceOnline.Hubs.Streams.JoinStream;

public class JoinStreamHandler: IRequestHandler<JoinStreamCommand, Unit>
{
    #region Fields

    private readonly IDataContext _context;

    private readonly IHubContext<StreamHub, IStreamHub> _streamHubContext;

    #endregion

    #region Construction

    public JoinStreamHandler(IHubContext<StreamHub, IStreamHub> streamHubContext, IDataContext context)
    {
        _streamHubContext = streamHubContext;
        _context = context;
    }

    #endregion

    #region Public Methods

    public async Task<Unit> Handle(JoinStreamCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        await _streamHubContext.Groups.AddToGroupAsync(command.ConnectionId, command.LobbyId, cancellationToken);

        command.ConnectionIdsToGroupNames[command.ConnectionId] = command.LobbyId;

        var user = await _context.Query<IUserByUserIdDbQuery>()
                                 .WithParams(command.UserId)
                                 .WithNoTracking()
                                 .ExecuteAsync(cancellationToken);
        var connection = new ConnectionModel
                         {
                             UserName = user.UserName,
                             ConnectionId = command.ConnectionId
                         };

        var streamGroupModel = command.GroupNameToGroupModel
                                      .GetOrAdd(command.LobbyId, _ => new StreamGroupModel
                                                                      {
                                                                          ConnectionIds = new List<ConnectionModel>(),
                                                                          ConnectionIdToCalledConnectionIds = new ConcurrentDictionary<string, List<string>>()
                                                                      });

        lock(streamGroupModel.ConnectionIds)
        {
            streamGroupModel.ConnectionIds.Add(connection);
        }

        streamGroupModel.ConnectionIdToCalledConnectionIds[command.ConnectionId] = new List<string>();


        // Use ConcurrentDictionary to handle concurrent access to the dictionary
        var listOfCalledIdsDictionary = new ConcurrentDictionary<string, List<string>>();

        foreach(var callerConnectionId in streamGroupModel.ConnectionIdToCalledConnectionIds.Keys)
        {
            var calledConnections = streamGroupModel.ConnectionIdToCalledConnectionIds.TryGetValue(callerConnectionId, out var listOfCalledIds);
            if (!calledConnections)
            {
                listOfCalledIds = new List<string>();
            }

            // Lock to ensure thread safety when updating listOfCalledIdsDictionary
            lock(listOfCalledIdsDictionary)
            {
                var keysOfConnectionsWhoHaveAlreadyCalled = streamGroupModel.ConnectionIdToCalledConnectionIds
                                                                            .Where(kv => kv.Value.Contains(callerConnectionId))
                                                                            .Distinct()
                                                                            .Select(kv => kv.Key);

                foreach(var kesOfConnectionsWhoHaveAlreadyCalled in keysOfConnectionsWhoHaveAlreadyCalled)
                {
                    listOfCalledIds.Add(kesOfConnectionsWhoHaveAlreadyCalled);
                }

                // Add to the ConcurrentDictionary
                listOfCalledIdsDictionary.TryAdd(callerConnectionId, listOfCalledIds);
            }

            foreach(var connectionIdToCall in streamGroupModel.ConnectionIdToCalledConnectionIds.Keys.Where(k => k != callerConnectionId))
            {
                // Lock to ensure thread safety when accessing streamGroup.ConnectionIds
                lock(streamGroupModel.ConnectionIds)
                {
                    if (!listOfCalledIds.Contains(connectionIdToCall))
                    {
                        listOfCalledIds.Add(connectionIdToCall);
                    }
                }
            }
        }

        foreach(var callerConnectionId in streamGroupModel.ConnectionIdToCalledConnectionIds)
        {
            foreach(var callConnection in callerConnectionId.Value.Distinct()
                                                            .Select(connectionIdToCall => streamGroupModel.ConnectionIds.FirstOrDefault(c => c.ConnectionId == connectionIdToCall))
                                                            .Where(callConnection => callConnection != null))
            {
                await _streamHubContext.Clients.Client(callerConnectionId.Key).InitiateCall(callConnection);
            }
        }

        return default;
    }

    #endregion
}
