using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace TheResistanceOnline.Hubs.Streams;

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

        var candidateModel = new CandidateModel
                             {
                                 ConnectionIdOfWhoSentCandidate = command.ConnectionId,
                                 RtcIceCandidate = command.RTCIceCandidate
                             };
        await _streamHubContext.Clients.Client(command.ConnectIdOfWhoCandidateIsFor).HandleCandidate(candidateModel);

        return default;
    }

    #endregion
}
