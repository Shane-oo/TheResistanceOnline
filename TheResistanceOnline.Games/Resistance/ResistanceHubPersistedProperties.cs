using System.Collections.Concurrent;

namespace TheResistanceOnline.Games.Resistance;

public class ResistanceHubPersistedProperties
{
    public readonly ConcurrentDictionary<string, string> _connectionIdsToGroupNames = new();
}
