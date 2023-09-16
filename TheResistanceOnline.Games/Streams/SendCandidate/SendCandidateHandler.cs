using MediatR;
using Microsoft.AspNetCore.SignalR;
using TheResistanceOnline.Core.Exceptions;
using TheResistanceOnline.Data.Entities.UserEntities;

namespace TheResistanceOnline.Games.Streams.SendCandidate;

public class SendCandidateHandler: IRequestHandler<SendCandidateCommand, Unit>
{
    #region Fields

    private readonly IHubContext<StreamHub, IStreamHub> _streamHubContext;

    #endregion

    #region Construction

    public SendCandidateHandler(IHubContext<StreamHub, IStreamHub> streamHubContext)
    {
        _streamHubContext = streamHubContext;
    }

    #endregion

    #region Public Methods

    public async Task<Unit> Handle(SendCandidateCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        UnauthorizedException.ThrowIfUserIsNotAllowedAccess(command, Roles.User);

        if (!command.ConnectionIdsToGroupNames.TryGetValue(command.ConnectionId, out var groupName))
        {
            throw new NotFoundException("Group Not Found");
        }

        await _streamHubContext.Clients.GroupExcept(groupName, command.ConnectionId).HandleCandidate(command.RTCIceCandidate);

        return default;
    }

    #endregion
}
