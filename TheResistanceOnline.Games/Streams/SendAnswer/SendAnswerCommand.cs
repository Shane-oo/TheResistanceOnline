using MediatR;
using TheResistanceOnline.Core.Requests.Commands;
using TheResistanceOnline.Games.Streams.Common;

namespace TheResistanceOnline.Games.Streams.SendAnswer;

public class SendAnswerCommand: CommandBase<Unit>
{
    #region Properties

    public RTCSessionDescriptionModel RTCSessionDescription { get; set; }

    public Dictionary<string, string> ConnectionIdsToGroupNames { get; set; }

    #endregion
}
