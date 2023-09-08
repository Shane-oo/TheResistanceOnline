using FluentValidation;
using MediatR;
using TheResistanceOnline.Core.Requests.Commands;

namespace TheResistanceOnline.Games.Lobbies.CreateLobby;

public class CreateLobbyCommand: CommandBase<Unit>
{
    #region Properties

    public string RoomId { get; set; }
    
    public bool PrivateRoom { get; set; }

    #endregion
}

public class CreateLobbyCommandValidator: AbstractValidator<CreateLobbyCommand>
{
    #region Construction

    public CreateLobbyCommandValidator()
    {
        RuleFor(c => c.RoomId)
            .NotEmpty()
            .NotNull()
            .Matches(@"^[A-Za-z0-9ñÑáéíóúÁÉÍÓÚ/^\S*$/]+$").WithMessage("Room Id can only contain letters and/or numbers")
            .Length(1, 12).WithMessage("Room Id Too Long");
    }

    #endregion
}
