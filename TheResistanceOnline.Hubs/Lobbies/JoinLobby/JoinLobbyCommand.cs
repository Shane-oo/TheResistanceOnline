using System.Collections.Concurrent;
using FluentValidation;
using JetBrains.Annotations;
using TheResistanceOnline.Core.Exchange.Requests;
using TheResistanceOnline.Hubs.Common;

namespace TheResistanceOnline.Hubs.Lobbies;

public class JoinLobbyCommand: Command<string>, IConnectionModel
{
    #region Properties

    public string ConnectionId { get; set; }

    public ConcurrentDictionary<string, LobbyDetailsModel> GroupNamesToLobby { get; set; }

    public string LobbyId { get; set; }

    #endregion
}

[UsedImplicitly]
public class JoinLobbyCommandValidator: AbstractValidator<JoinLobbyCommand>
{
    #region Construction

    public JoinLobbyCommandValidator()
    {
        RuleFor(c => c.LobbyId)
            .NotEmpty();

        RuleFor(c => c.ConnectionId)
            .NotEmpty();
    }

    #endregion
}
