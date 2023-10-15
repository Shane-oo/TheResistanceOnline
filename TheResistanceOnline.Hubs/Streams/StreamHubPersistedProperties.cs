using System.Collections.Concurrent;

namespace TheResistanceOnline.Hubs.Streams;

public class StreamHubPersistedProperties
{
    #region Fields

    public readonly ConcurrentDictionary<string, string> _connectionIdsToGroupNames = new();

    #endregion
}
