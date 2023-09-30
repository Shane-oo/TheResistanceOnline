using TheResistanceOnline.Hubs.Streams.Common;

namespace TheResistanceOnline.Hubs.Streams.SendAnswer;

public class AnswerModel
{
    #region Properties

    public string ConnectionIdOfWhoAnswered { get; set; }

    public RTCSessionDescriptionModel RtcSessionDescription { get; set; }

    #endregion
}
