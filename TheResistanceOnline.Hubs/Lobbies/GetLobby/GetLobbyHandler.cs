using TheResistanceOnline.Core.Errors;
using TheResistanceOnline.Core.NewCommandAndQueriesAndResultsPattern;

namespace TheResistanceOnline.Hubs.Lobbies;

public class GetLobbyHandler: IQueryHandler<GetLobbyQuery, LobbyDetailsModel>
{
    #region Public Methods

    public async Task<Result<LobbyDetailsModel>> Handle(GetLobbyQuery query, CancellationToken cancellationToken)
    {
        if (query == null)
        {
            return Result.Failure<LobbyDetailsModel>(Error.NullValue);
        }

        return !query.GroupNamesToLobby.TryGetValue(query.Id, out var lobbyDetails)
                   ? Result.Failure<LobbyDetailsModel>(NotFoundError.NotFound(query.Id))
                   : lobbyDetails;
    }

    #endregion
}
