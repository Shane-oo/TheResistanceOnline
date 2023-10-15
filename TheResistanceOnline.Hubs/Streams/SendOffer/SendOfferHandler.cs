using MediatR;
using Microsoft.AspNetCore.SignalR;
using TheResistanceOnline.Core.Exceptions;
using TheResistanceOnline.Data.Entities.UserEntities;

namespace TheResistanceOnline.Hubs.Streams.SendOffer;

public class SendOfferHandler: IRequestHandler<SendOfferCommand, Unit>
{
    #region Fields

    private readonly IHubContext<StreamHub, IStreamHub> _streamHubContext;

    #endregion

    #region Construction

    public SendOfferHandler(IHubContext<StreamHub, IStreamHub> streamHubContext)
    {
        _streamHubContext = streamHubContext;
    }

    #endregion

    #region Public Methods

    public async Task<Unit> Handle(SendOfferCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        UnauthorizedException.ThrowIfUserIsNotAllowedAccess(command, Roles.User);
     
        var offer = new OfferModel
                    {
                        ConnectionIdOfWhoOffered = command.ConnectionId,
                        RtcSessionDescription = command.RTCSessionDescription
                    };
        await _streamHubContext.Clients.Client(command.ConnectionIdOfWhoOfferIsFor).HandleOffer(offer);

        return default;
    }

    #endregion
}