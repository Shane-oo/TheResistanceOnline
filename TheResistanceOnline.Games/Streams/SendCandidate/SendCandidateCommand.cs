using MediatR;
using TheResistanceOnline.Core.Requests.Commands;
using TheResistanceOnline.Games.Streams.Common;

namespace TheResistanceOnline.Games.Streams.SendCandidate;

public class SendCandidateCommand: CommandBase<Unit>
{
    #region Properties

    public string ConnectIdOfWhoCandidateIsFor { get; set; }

    public Dictionary<string, string> ConnectionIdsToGroupNames { get; set; }

    public RTCIceCandidateModel RTCIceCandidate { get; set; }

    #endregion
}
