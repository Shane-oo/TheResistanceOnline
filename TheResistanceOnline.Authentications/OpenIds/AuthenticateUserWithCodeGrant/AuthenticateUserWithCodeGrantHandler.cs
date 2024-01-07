using TheResistanceOnline.Core.Errors;
using TheResistanceOnline.Core.Exchange.Requests;
using TheResistanceOnline.Core.Exchange.Responses;
using TheResistanceOnline.Core.NewCommandAndQueriesAndResultsPattern;
using TheResistanceOnline.Data;
using TheResistanceOnline.Data.Entities;
using TheResistanceOnline.Data.Queries;

namespace TheResistanceOnline.Authentications.OpenIds;

public class AuthenticateUserWithCodeGrantHandler:
    ICommandHandler<AuthenticateUserWithCodeGrantCommand, UserAuthenticationPayload>
{
    #region Fields

    private readonly IDataContext _context;

    #endregion

    #region Construction

    public AuthenticateUserWithCodeGrantHandler(IDataContext context)
    {
        _context = context;
    }

    #endregion

    #region Public Methods

    public async Task<Result<UserAuthenticationPayload>> Handle(AuthenticateUserWithCodeGrantCommand command, CancellationToken cancellationToken)
    {
        if (command == null)
        {
            return Result.Failure<UserAuthenticationPayload>(Error.NullValue);
        }

        var user = await _context.Query<IUserByUserIdDbQuery>()
                                 .Include($"{nameof(User.UserRole)}.{nameof(UserRole.Role)}")
                                 .WithParams(command.UserId)
                                 .ExecuteAsync(cancellationToken);

        if (user is null)
        {
            return Result.Failure<UserAuthenticationPayload>(OpenIdErrors.UserNotFound);
        }

        user.LoginOn = DateTimeOffset.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return new UserAuthenticationPayload
               {
                   UserId = user.Id,
                   UserName = user.UserName,
                   Role = user.UserRole.Role.Name
               };
    }

    #endregion
}
