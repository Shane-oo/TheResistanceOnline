using TheResistanceOnline.Games.Streams.Common;

namespace TheResistanceOnline.Games.Streams.SendCandidate;

public class CandidateModel
{
    #region Properties

    public string ConnectionIdOfWhoSentCandidate { get; set; }

    public RTCIceCandidateModel RtcIceCandidate { get; set; }

    #endregion
}
