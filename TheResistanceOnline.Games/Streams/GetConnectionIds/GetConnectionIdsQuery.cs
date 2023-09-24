using MediatR;
using TheResistanceOnline.Core.Requests.Queries;

namespace TheResistanceOnline.Games.Streams.GetPeerConnections;

public class GetConnectionIdsQuery: QueryBase<List<string>>
{
    #region Properties

    public Dictionary<string, string> ConnectionIdsToGroupNames { get; set; }

    #endregion
}
