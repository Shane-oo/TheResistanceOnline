using System.Runtime.CompilerServices;

namespace TheResistanceOnline.Core.Exceptions;

public class NotFoundException: Exception
{
    #region Construction

    public NotFoundException()
    {
    }

    public NotFoundException(string message): base(message)
    {
    }

    #endregion

    #region Private Methods

    private static void Throw(string? paramName)
    {
        throw new NotFoundException(string.Concat(paramName, " ", "Not Found"));
    }

    #endregion

    #region Public Methods

    public static void ThrowIfNull(object argument, [CallerArgumentExpression("argument")] string paramName = null)
    {
        if (argument is null)
        {
            Throw(paramName);
        }
    }

    #endregion
}
