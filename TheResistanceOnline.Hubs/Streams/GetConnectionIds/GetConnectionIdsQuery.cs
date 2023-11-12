using System.Collections.Concurrent;
using TheResistanceOnline.Core.Requests.Queries;
using TheResistanceOnline.Hubs.Common;
using TheResistanceOnline.Hubs.Streams.Common;

namespace TheResistanceOnline.Hubs.Streams.GetConnectionIds;

public class GetConnectionIdsQuery: QueryBase<List<ConnectionModel>>
{
    #region Properties

    public StreamGroupModel StreamGroupModel { get; set; }
    
    #endregion
}
