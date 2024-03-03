
namespace TheResistanceOnline.Server.Streams;

public class OfferModel
{
    public RTCSessionDescriptionModel RtcSessionDescription { get; set; }

    public string ConnectionIdOfWhoOffered { get; set; }
}
