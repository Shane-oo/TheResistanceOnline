using FluentValidation;
using TheResistanceOnline.Core.Errors;
using TheResistanceOnline.Core.NewCommandAndQueriesAndResultsPattern;

namespace TheResistanceOnline.Hubs.Lobbies;

public class GetLobbyHandler: IQueryHandler<GetLobbyQuery, LobbyDetailsModel>
{
    #region Fields

    private readonly IValidator<GetLobbyQuery> _validator;

    #endregion

    #region Construction

    public GetLobbyHandler(IValidator<GetLobbyQuery> validator)
    {
        _validator = validator;
    }

    #endregion

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
