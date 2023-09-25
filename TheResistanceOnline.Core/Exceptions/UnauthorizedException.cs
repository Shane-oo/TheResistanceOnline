using TheResistanceOnline.Common.Extensions;
using TheResistanceOnline.Core.Requests;
using TheResistanceOnline.Data.Entities.UserEntities;

namespace TheResistanceOnline.Core.Exceptions;

public class UnauthorizedException: Exception
{
    #region Private Methods

    private static void Throw()
    {
        throw new UnauthorizedException();
    }

    #endregion

    #region Public Methods

    public static void ThrowIfUserIsNotAllowedAccess(IRequestBase request, Roles requiredRole)
    {
        // Check if user id is valid
        if (!request.UserId.HasValue()) Throw();

        switch(requiredRole)
        {
            case Roles.Admin:
                if (request.UserRole != Roles.Admin)
                {
                    Throw();
                }

                break;
            case Roles.Moderator:
                if (request.UserRole != Roles.Admin && request.UserRole != Roles.Moderator)
                {
                    Throw();
                }

                break;
            case Roles.User:
                if (request.UserRole != Roles.Admin && request.UserRole != Roles.Moderator && request.UserRole != Roles.User)
                {
                    Throw();
                }

                break;
            case Roles.None:
                // No role is required for this action (Anonymous)
                break;
        }
    }

    #endregion
}
