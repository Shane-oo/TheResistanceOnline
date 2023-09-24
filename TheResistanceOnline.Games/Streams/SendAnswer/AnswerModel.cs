using TheResistanceOnline.Games.Streams.Common;

namespace TheResistanceOnline.Games.Streams.SendAnswer;

public class AnswerModel
{
    #region Properties

    public string ConnectionIdOfWhoAnswered { get; set; }

    public RTCSessionDescriptionModel RtcSessionDescription { get; set; }

    #endregion
}
