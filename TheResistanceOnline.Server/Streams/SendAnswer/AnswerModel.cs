namespace TheResistanceOnline.Server.Streams;

public class AnswerModel
{
    #region Properties

    public string ConnectionIdOfWhoAnswered { get; set; }

    public RTCSessionDescriptionModel RtcSessionDescription { get; set; }

    #endregion
}
