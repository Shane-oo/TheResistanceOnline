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
