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
