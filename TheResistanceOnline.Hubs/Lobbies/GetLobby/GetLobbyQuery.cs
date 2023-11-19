using System.Collections.Concurrent;
using FluentValidation;
using TheResistanceOnline.Core.Requests.Queries;

namespace TheResistanceOnline.Hubs.Lobbies;

public class GetLobbyQuery: QueryBase<LobbyDetailsModel>
{
    #region Properties

    public string Id { get; set; }

    public ConcurrentDictionary<string, LobbyDetailsModel> GroupNamesToLobby { get; set; }

    #endregion
}

public class SearchLobbyQueryValidator: AbstractValidator<GetLobbyQuery>
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
