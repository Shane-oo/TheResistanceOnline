using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;
using TheResistanceOnline.GamePlay.Common;
using TheResistanceOnline.GamePlay.GameModels;

namespace TheResistanceOnline.Server.Resistance;

public class ResistanceHubPersistedProperties
{
    #region Fields

    public readonly ConcurrentDictionary<string, string> _connectionIdsToGroupNames = new();

    public readonly ConcurrentDictionary<string, GameDetails> _groupNamesToGameModels = new();

    #endregion
}

