using System.Collections.Concurrent;

namespace TheResistanceOnline.Hubs.Resistance;

public class ResistanceHubPersistedProperties
{
    #region Fields

    public readonly ConcurrentDictionary<string, string> _connectionIdsToGroupNames = new();

    public readonly ConcurrentDictionary<string, GameDetails> _groupNamesToGameModels = new();

    #endregion
}
