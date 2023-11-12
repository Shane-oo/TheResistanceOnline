using System.Collections.Concurrent;
using TheResistanceOnline.Hubs.Common;

namespace TheResistanceOnline.Hubs.Streams.Common;

public class StreamGroupModel
{
    public ConcurrentDictionary<string, List<string>> ConnectionIdToCalledConnectionIds { get; set; }
    
    public List<ConnectionModel> ConnectionIds { get; set; }
}
