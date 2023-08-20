using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TheResistanceOnline.Core.Exceptions;
using TheResistanceOnline.Data;
using TheResistanceOnline.Data.Entities.UserEntities;
using TheResistanceOnline.Data.Queries.UserQueries;

namespace TheResistanceOnline.Users.Users.UpdateUser;

public class UpdateUserHandler: IRequestHandler<UpdateUserCommand, Unit>
{
    #region Fields

    private readonly IDataContext _context;
    private readonly UserManager<User> _userManager;
    private readonly IValidator<UpdateUserCommand> _validator;

    #endregion

    #region Construction

    public UpdateUserHandler(IDataContext context, UserManager<User> userManager, IValidator<UpdateUserCommand> validator)
    {
        _context = context;
        _userManager = userManager;
        _validator = validator;
    }

    #endregion

    #region Public Methods

    public async Task<Unit> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        UnauthorizedException.ThrowIfUserIsNotAllowedAccess(command, Roles.User);

        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid) throw new DomainException(validationResult.Errors.First().ErrorMessage);

        var user = await _context.Query<IUserByUserIdDbQuery>()
                                 .WithParams(command.UserId)
                                 .ExecuteAsync(cancellationToken);

        NotFoundException.ThrowIfNull(user);

        user.UserName = command.UserName.Trim();

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            var errorDescription = result.Errors.FirstOrDefault()?.Description;
            throw new DomainException(errorDescription);
        }

        return default;
    }

    #endregion
}
