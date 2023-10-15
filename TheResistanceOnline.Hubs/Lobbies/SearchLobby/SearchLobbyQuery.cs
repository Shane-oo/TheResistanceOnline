using System.Collections.Concurrent;
using FluentValidation;
using TheResistanceOnline.Core.Requests.Queries;
using TheResistanceOnline.Hubs.Lobbies.Common;

namespace TheResistanceOnline.Hubs.Lobbies.SearchLobby;

public class SearchLobbyQuery: QueryBase<string>
{
    #region Properties

    public string Id { get; set; }

    public ConcurrentDictionary<string, LobbyDetailsModel> GroupNamesToLobby { get; set; }

    #endregion
}

public class SearchLobbyQueryValidator: AbstractValidator<SearchLobbyQuery>
{
    #region Construction

    public SearchLobbyQueryValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty()
            .NotNull()
            .Matches("^[a-zA-Z0-9]+$").WithMessage("Lobby Id can only contain letters and/or numbers")
            .Length(1, 20).WithMessage("Lobby Id Too Long");
    }

    #endregion
}
