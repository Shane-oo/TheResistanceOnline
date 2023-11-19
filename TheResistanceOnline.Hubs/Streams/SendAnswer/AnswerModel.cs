using TheResistanceOnline.Hubs.Streams;

namespace TheResistanceOnline.Hubs.Streams;

public class AnswerModel
{
    #region Properties

    public string ConnectionIdOfWhoAnswered { get; set; }

    public RTCSessionDescriptionModel RtcSessionDescription { get; set; }

    #endregion
}
