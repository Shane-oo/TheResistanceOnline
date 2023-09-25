using System.Collections.Concurrent;
using TheResistanceOnline.Core.Requests.Queries;

namespace TheResistanceOnline.Games.Streams.GetConnectionIds;

public class GetConnectionIdsQuery: QueryBase<List<string>>
{
    #region Properties

    public ConcurrentDictionary<string, string> ConnectionIdsToGroupNames { get; set; }

    #endregion
}
