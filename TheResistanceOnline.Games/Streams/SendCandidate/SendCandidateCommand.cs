using System.Collections.Concurrent;
using MediatR;
using TheResistanceOnline.Core.Requests.Commands;

namespace TheResistanceOnline.Games.Streams.SendCandidate;

public class SendCandidateCommand: CommandBase<Unit>
{
    #region Properties

    public string ConnectIdOfWhoCandidateIsFor { get; set; }

    public RTCIceCandidateModel RTCIceCandidate { get; set; }

    #endregion
}
