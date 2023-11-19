using FluentValidation;
using MediatR;
using TheResistanceOnline.Core.Requests.Commands;

namespace TheResistanceOnline.Users.Users;

public class UpdateUserCommand: CommandBase<Unit>
{
    #region Properties

    public string UserName { get; set; }

    #endregion
}

public class UpdateUserCommandValidator: AbstractValidator<UpdateUserCommand>
{
    #region Construction

    public UpdateUserCommandValidator()
    {
        RuleFor(c => c.UserName)
            .NotEmpty()
            .NotNull()
            .Matches("^[a-zA-Z0-9]+$").WithMessage("UserName can only contain letters and/or numbers")
            .Must(u => !u.ToLower().StartsWith("bot")).WithMessage("'bot' is a reserved name")
            .Length(1, 31).WithMessage("Username Too Long");
    }

    #endregion
}
