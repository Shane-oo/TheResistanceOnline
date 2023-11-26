namespace TheResistanceOnline.Hubs.Streams;

public class CandidateModel
{
    #region Properties

    public string ConnectionIdOfWhoSentCandidate { get; set; }

    public RTCIceCandidateModel RtcIceCandidate { get; set; }

    #endregion
}
