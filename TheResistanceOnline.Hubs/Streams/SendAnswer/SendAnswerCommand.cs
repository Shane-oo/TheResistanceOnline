using MediatR;
using TheResistanceOnline.Core.Requests.Commands;
using TheResistanceOnline.Hubs.Streams.Common;

namespace TheResistanceOnline.Hubs.Streams.SendAnswer;

public class SendAnswerCommand: CommandBase<Unit>
{
    #region Properties

    public string ConnectionIdOfWhoAnswerIsFor { get; set; }

    public RTCSessionDescriptionModel RtcSessionDescription { get; set; }

    #endregion
}
