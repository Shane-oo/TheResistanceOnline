using MediatR;
using TheResistanceOnline.Core.Requests.Commands;

namespace TheResistanceOnline.Hubs.Streams;

public class SendCandidateCommand: CommandBase<Unit>
{
    #region Properties

    public string ConnectIdOfWhoCandidateIsFor { get; set; }

    public RTCIceCandidateModel RTCIceCandidate { get; set; }

    #endregion
}
