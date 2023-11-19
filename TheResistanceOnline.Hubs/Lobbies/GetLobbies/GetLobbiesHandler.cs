using MediatR;
using TheResistanceOnline.Hubs.Lobbies;

namespace TheResistanceOnline.Hubs.Lobbies;

public class GetLobbiesHandler: IRequestHandler<GetLobbiesQuery, List<LobbyDetailsModel>>
{
    #region Public Methods

    public async Task<List<LobbyDetailsModel>> Handle(GetLobbiesQuery query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        var lobbies = query.GroupNamesToLobby.Values
                           .Where(l => !l.IsPrivate)
                           .ToList();

        return lobbies;
    }

    #endregion
}
