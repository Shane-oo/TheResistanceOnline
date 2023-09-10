using FluentValidation;
using MediatR;
using TheResistanceOnline.Core.Requests.Commands;
using TheResistanceOnline.Games.Lobbies.Common;

namespace TheResistanceOnline.Games.Lobbies.CreateLobby;

public class CreateLobbyCommand: CommandBase<LobbyDetailsModel>
{
    #region Properties

    public string Id { get; set; }

    public bool IsPrivate { get; set; }

    public Dictionary<string, LobbyDetailsModel> GroupNamesToLobby { get; set; }

    #endregion
}

public class CreateLobbyCommandValidator: AbstractValidator<CreateLobbyCommand>
{
    #region Construction

    public CreateLobbyCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty()
            .NotNull()
            .Matches(@"^[A-Za-z0-9ñÑáéíóúÁÉÍÓÚ/^\S*$/]+$").WithMessage("Lobby Id can only contain letters and/or numbers")
            .Length(1, 20).WithMessage("Lobby Id Too Long");
    }

    #endregion
}
