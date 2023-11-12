using MediatR;
using TheResistanceOnline.Data;
using TheResistanceOnline.Data.Entities.UserEntities;
using TheResistanceOnline.Data.Queries.UserQueries;

namespace TheResistanceOnline.Authentications.OpenIds;

public class AuthenticateUserWithCodeGrantHandler:
    IRequestHandler<AuthenticateUserWithCodeGrantCommand, AuthenticationResult<UserAuthenticationPayload>>
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

    #region Private Methods

    private static AuthenticationResult<UserAuthenticationPayload> Reject(string reason)
    {
        return AuthenticationResult<UserAuthenticationPayload>.Reject(reason);
    }

    #endregion

    #region Public Methods

    public async Task<AuthenticationResult<UserAuthenticationPayload>> Handle(AuthenticateUserWithCodeGrantCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var user = await _context.Query<IUserByUserIdDbQuery>()
                                 .Include($"{nameof(User.UserRole)}.{nameof(UserRole.Role)}")
                                 .WithParams(command.UserId)
                                 .ExecuteAsync(cancellationToken);

        if (user is null)
        {
            return Reject("Invalid User.");
        }

        user.LoginOn = DateTimeOffset.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        var result = new UserAuthenticationPayload
                     {
                         UserId = user.Id,
                         UserName = user.UserName,
                         Role = user.UserRole.Role.Name
                     };
        return AuthenticationResult<UserAuthenticationPayload>.Accept(result);
    }

    #endregion
}
