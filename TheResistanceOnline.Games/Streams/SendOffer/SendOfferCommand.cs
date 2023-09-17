using MediatR;
using TheResistanceOnline.Core.Requests.Commands;
using TheResistanceOnline.Games.Streams.Common;

namespace TheResistanceOnline.Games.Streams.CreateOffer;

public class SendOfferCommand: CommandBase<Unit>
{
    #region Properties

    public string ConnectionIdOfWhoOfferIsFor { get; set; }

    public RTCSessionDescriptionModel RTCSessionDescription { get; set; }

    #endregion
}
