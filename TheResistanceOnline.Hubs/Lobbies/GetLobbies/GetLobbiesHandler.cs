using MediatR;
using Microsoft.AspNetCore.SignalR;
using TheResistanceOnline.Core.Exceptions;
using TheResistanceOnline.Data.Entities.UserEntities;
using TheResistanceOnline.Games.Lobbies;
using TheResistanceOnline.Hubs.Lobbies.Common;

namespace TheResistanceOnline.Hubs.Lobbies.GetLobbies;

public class GetLobbiesHandler: IRequestHandler<GetLobbiesQuery, List<LobbyDetailsModel>>
{
    #region Fields

    private readonly IHubContext<LobbyHub, ILobbyHub> _lobbyHubContext;

    #endregion

    #region Construction

    public GetLobbiesHandler(IHubContext<LobbyHub, ILobbyHub> lobbyHubContext)
    {
        _lobbyHubContext = lobbyHubContext;
    }

    #endregion

    #region Public Methods

    public Task<List<LobbyDetailsModel>> Handle(GetLobbiesQuery query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        UnauthorizedException.ThrowIfUserIsNotAllowedAccess(query, Roles.User);

        var lobbies = query.GroupNamesToLobby.Values
                           .Where(l => !l.IsPrivate)
                           .ToList();

        return Task.FromResult(lobbies);
    }

    #endregion
}
