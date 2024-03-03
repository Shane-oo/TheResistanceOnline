using TheResistanceOnline.Core.Errors;
using TheResistanceOnline.Core.Exchange.Requests;
using TheResistanceOnline.Core.Exchange.Responses;

namespace TheResistanceOnline.Server.Lobbies;

public class GetLobbiesHandler: IQueryHandler<GetLobbiesQuery, List<LobbyDetailsModel>>
{
    #region Public Methods

    public async Task<Result<List<LobbyDetailsModel>>> Handle(GetLobbiesQuery query, CancellationToken cancellationToken)
    {
        if (query == null)
        {
            return Result.Failure<List<LobbyDetailsModel>>(Error.NullValue);
        }

        var lobbies = query.GroupNamesToLobby.Values
                           .Where(l => !l.IsPrivate)
                           .ToList();

        return lobbies;
    }

    #endregion
}
