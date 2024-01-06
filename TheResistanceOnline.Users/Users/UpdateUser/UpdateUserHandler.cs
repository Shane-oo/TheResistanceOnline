using Microsoft.AspNetCore.Identity;
using TheResistanceOnline.Core.Errors;
using TheResistanceOnline.Core.Exchange.Requests;
using TheResistanceOnline.Core.Exchange.Responses;
using TheResistanceOnline.Core.NewCommandAndQueriesAndResultsPattern;
using TheResistanceOnline.Data;
using TheResistanceOnline.Data.Entities;
using TheResistanceOnline.Data.Queries;

namespace TheResistanceOnline.Users.Users;

public class UpdateUserHandler: ICommandHandler<UpdateUserCommand>
{
    #region Fields

    private readonly IDataContext _context;
    private readonly UserManager<User> _userManager;

    #endregion

    #region Construction

    public UpdateUserHandler(IDataContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    #endregion

    #region Public Methods

    public async Task<Result> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        if (command == null)
        {
            return Result.Failure(Error.NullValue);
        }

        var user = await _context.Query<IUserByUserIdDbQuery>()
                                 .WithParams(command.UserId)
                                 .ExecuteAsync(cancellationToken);

        var notFoundResult = NotFoundError.FailIfNull(user);
        if (notFoundResult.IsFailure)
        {
            return notFoundResult;
        }

        user.UserName = command.UserName.Trim();

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            var identityError = result.Errors.FirstOrDefault();
            return Result.Failure(identityError != null ? new Error(identityError.Code, identityError.Description) : Error.Unknown);
        }

        return Result.Success();
    }

    #endregion
}
