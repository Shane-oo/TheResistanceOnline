using System.Collections.Concurrent;
using TheResistanceOnline.Hubs.Streams.Common;

namespace TheResistanceOnline.Hubs.Streams;

public class StreamHubPersistedProperties
{
    #region Fields

    public readonly ConcurrentDictionary<string, string> _connectionIdsToGroupNames = new();

    public readonly ConcurrentDictionary<string, StreamGroupModel> _groupNameToGroupModel = new();

    #endregion
}
