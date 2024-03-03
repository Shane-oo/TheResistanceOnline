using System.Collections.Concurrent;
using FluentValidation;
using JetBrains.Annotations;
using TheResistanceOnline.Core.Exchange.Requests;
using TheResistanceOnline.Server.Common;

namespace TheResistanceOnline.Server.Lobbies;

public class CreateLobbyCommand: Command<string>, IConnectionModel
{
    #region Properties

    public string ConnectionId { get; set; }

    public bool FillWithBots { get; set; }

    public ConcurrentDictionary<string, LobbyDetailsModel> GroupNamesToLobby { get; set; }

    public string Id { get; set; }

    public bool IsPrivate { get; set; }

    public int MaxPlayers { get; set; }

    #endregion
}

[UsedImplicitly]
public class CreateLobbyCommandValidator: AbstractValidator<CreateLobbyCommand>
{
    #region Construction

    public CreateLobbyCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty()
            .Matches("^[a-zA-Z0-9]+$").WithMessage("Lobby Id can only contain letters and/or numbers")
            .Length(1, 20).WithMessage("Lobby Id Too Long");

        RuleFor(c => c.MaxPlayers)
            .NotNull()
            .LessThanOrEqualTo(10)
            .GreaterThanOrEqualTo(5);

        RuleFor(c => c.ConnectionId)
            .NotEmpty();
    }

    #endregion
}
