using System.Collections.Concurrent;
using TheResistanceOnline.Games.Streams.Common;

namespace TheResistanceOnline.Games.Streams;

public class StreamHubPersistedProperties
{
    #region Fields

    public readonly ConcurrentDictionary<string, string> _connectionIdsToGroupNames = new();

    #endregion
}
