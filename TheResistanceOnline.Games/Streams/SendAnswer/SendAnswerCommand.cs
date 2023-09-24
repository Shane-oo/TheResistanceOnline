using MediatR;
using TheResistanceOnline.Core.Requests.Commands;
using TheResistanceOnline.Games.Streams.Common;

namespace TheResistanceOnline.Games.Streams.SendAnswer;

public class SendAnswerCommand: CommandBase<Unit>
{
    #region Properties

    public string ConnectionIdOfWhoAnswerIsFor { get; set; }

    public RTCSessionDescriptionModel RtcSessionDescription { get; set; }

    #endregion
}
