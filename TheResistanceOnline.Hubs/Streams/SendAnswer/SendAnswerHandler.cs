using MediatR;
using Microsoft.AspNetCore.SignalR;
using TheResistanceOnline.Core.Exceptions;
using TheResistanceOnline.Data.Entities.UserEntities;

namespace TheResistanceOnline.Hubs.Streams.SendAnswer;

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

        var answer = new AnswerModel
                     {
                         ConnectionIdOfWhoAnswered = command.ConnectionId,
                         RtcSessionDescription = command.RtcSessionDescription
                     };
        await _streamHubContext.Clients.Client(command.ConnectionIdOfWhoAnswerIsFor).HandleAnswer(answer);

        return default;
    }

    #endregion
}
