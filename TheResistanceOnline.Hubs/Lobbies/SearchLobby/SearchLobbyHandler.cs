using FluentValidation;
using MediatR;
using TheResistanceOnline.Core.Exceptions;
using TheResistanceOnline.Data.Entities.UserEntities;

namespace TheResistanceOnline.Hubs.Lobbies.SearchLobby;

public class SearchLobbyHandler: IRequestHandler<SearchLobbyQuery, string>
{
    #region Fields

    private readonly IValidator<SearchLobbyQuery> _validator;

    #endregion

    #region Construction

    public SearchLobbyHandler(IValidator<SearchLobbyQuery> validator)
    {
        _validator = validator;
    }

    #endregion

    #region Public Methods

    public async Task<string> Handle(SearchLobbyQuery command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        UnauthorizedException.ThrowIfUserIsNotAllowedAccess(command, Roles.User);

        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid) throw new DomainException(validationResult.Errors.First().ErrorMessage);

        if (!command.GroupNamesToLobby.TryGetValue(command.Id, out var lobbyDetails))
        {
            throw new NotFoundException($"{command.Id} Not Found");
        }

        return lobbyDetails.Id;
    }

    #endregion
}
