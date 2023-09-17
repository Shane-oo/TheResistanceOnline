using TheResistanceOnline.Games.Streams.Common;

namespace TheResistanceOnline.Games.Streams;

public class StreamHubPersistedProperties
{
    #region Fields

    public readonly Dictionary<string, string> _connectionIdsToGroupNames = new();

    public readonly Dictionary<string, StreamChoice> _connectionIdsToStreamChoice = new();

    // "Lobby1": {
    //      "foo": ["bar","baz","qux"]   - foo needs to call bar baz and qux
    //      "bar": ["baz","qux"]  -- bar needs to call baz and qux
    //      "baz": ["qux"] -- baz needs to call qux
    //}
    public readonly Dictionary<string, Dictionary<string, List<string>>> _groupNamesToConnectionIdsToPeerConnectionIds = new();

    #endregion
}
