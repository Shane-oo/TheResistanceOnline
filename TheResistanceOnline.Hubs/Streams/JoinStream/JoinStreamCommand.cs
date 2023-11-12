using System.Collections.Concurrent;
using MediatR;
using TheResistanceOnline.Core.Requests.Commands;
using TheResistanceOnline.Hubs.Streams.Common;

namespace TheResistanceOnline.Hubs.Streams.JoinStream;

public class JoinStreamCommand: CommandBase<Unit>
{
    #region Properties

    public ConcurrentDictionary<string, string> ConnectionIdsToGroupNames { get; set; }


    public ConcurrentDictionary<string, StreamGroupModel> GroupNameToGroupModel { get; set; }

    public string LobbyId { get; set; }

    #endregion
}
