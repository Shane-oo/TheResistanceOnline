using TheResistanceOnline.Games.Streams.Common;

namespace TheResistanceOnline.Games.Streams.SendOffer;

public class OfferModel
{
    public RTCSessionDescriptionModel RtcSessionDescription { get; set; }

    public string ConnectionIdOfWhoOffered { get; set; }
}
