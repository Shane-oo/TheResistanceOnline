using MediatR;
using Microsoft.AspNetCore.SignalR;
using TheResistanceOnline.Core.Exceptions;
using TheResistanceOnline.Data.Entities.UserEntities;

namespace TheResistanceOnline.Games.Streams.SendAnswer;

public class SendAnswerHandler: IRequestHandler<SendAnswerCommand, Unit>
{
    #region Fields

    private readonly IHubContext<StreamHub, IStreamHub> _streamHubContext;

    #endregion

    #region Construction

    public SendAnswerHandler(IHubContext<StreamHub, IStreamHub> streamHubContext)
    {
        _streamHubContext = streamHubContext;
    }

    #endregion

    #region Public Methods

    public async Task<Unit> Handle(SendAnswerCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        UnauthorizedException.ThrowIfUserIsNotAllowedAccess(command, Roles.User);

        if (!command.ConnectionIdsToGroupNames.TryGetValue(command.ConnectionId, out var groupName))
        {
            throw new NotFoundException("Group Not Found");
        }

        await _streamHubContext.Clients.GroupExcept(groupName, command.ConnectionId).HandleAnswer(command.RTCSessionDescription);

        return default;
    }

    #endregion
}
