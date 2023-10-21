using FluentValidation;
using MediatR;
using TheResistanceOnline.Core.Exceptions;
using TheResistanceOnline.Hubs.Lobbies.Common;

namespace TheResistanceOnline.Hubs.Lobbies.GetLobby;

public class GetLobbyHandler: IRequestHandler<GetLobbyQuery, LobbyDetailsModel>
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

    public async Task<LobbyDetailsModel> Handle(GetLobbyQuery command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid) throw new DomainException(validationResult.Errors.First().ErrorMessage);

        if (!command.GroupNamesToLobby.TryGetValue(command.Id, out var lobbyDetails))
        {
            throw new NotFoundException($"{command.Id} Not Found");
        }

        return lobbyDetails;
    }

    #endregion
}
