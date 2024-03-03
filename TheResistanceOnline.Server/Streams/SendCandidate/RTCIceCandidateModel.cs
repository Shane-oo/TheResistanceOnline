namespace TheResistanceOnline.Server.Streams;

public class RTCIceCandidateModel
{
    #region Properties

    public string Candidate { get; set; }

    public string SdpMid { get; set; }

    public int SdpMLineIndex { get; set; }

    public string UsernameFragment { get; set; }

    #endregion
}
