using TheResistanceOnline.Hubs.Streams.Common;

namespace TheResistanceOnline.Hubs.Streams.SendOffer;

public class OfferModel
{
    public RTCSessionDescriptionModel RtcSessionDescription { get; set; }

    public string ConnectionIdOfWhoOffered { get; set; }
}
