using MediatR;
using TheResistanceOnline.Core.Requests.Commands;
using TheResistanceOnline.Hubs.Streams.Common;

namespace TheResistanceOnline.Hubs.Streams.SendOffer;

public class SendOfferCommand: CommandBase<Unit>
{
    #region Properties

    public string ConnectionIdOfWhoOfferIsFor { get; set; }

    public RTCSessionDescriptionModel RTCSessionDescription { get; set; }

    #endregion
}
