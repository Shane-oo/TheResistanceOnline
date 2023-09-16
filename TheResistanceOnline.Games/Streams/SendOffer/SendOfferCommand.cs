using MediatR;
using TheResistanceOnline.Core.Requests.Commands;
using TheResistanceOnline.Games.Streams.Common;

namespace TheResistanceOnline.Games.Streams.CreateOffer;

public class SendOfferCommand: CommandBase<Unit>
{
    #region Properties

    public RTCSessionDescriptionModel RTCSessionDescription { get; set; }

    public Dictionary<string, string> ConnectionIdsToGroupNames { get; set; }

    #endregion
}
