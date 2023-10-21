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
}
