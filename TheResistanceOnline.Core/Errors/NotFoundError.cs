using System.Runtime.CompilerServices;
using TheResistanceOnline.Core.Exchange.Responses;
using TheResistanceOnline.Core.NewCommandAndQueriesAndResultsPattern;

namespace TheResistanceOnline.Core.Errors;

public static class NotFoundError
{
    #region Private Methods

    private static Result Fail(string paramName)
    {
        return Result.Failure(NotFound(paramName));
    }

    #endregion

    #region Public Methods

    public static Result FailIfNull(object argument, [CallerArgumentExpression("argument")] string paramName = null)
    {
        return argument is null ? Fail(paramName) : Result.Success();
    }


    public static Result FailIfNullOrEmpty<T>(List<T> argument, [CallerArgumentExpression("argument")] string paramName = null)
    {
        if (argument is null || argument.Count == 0)
        {
            return Fail(paramName);
        }

        return Result.Success();
    }

    public static Error NotFound(string propertyName)
    {
        return new Error("Error.NotFound", $"{propertyName} was not found");
    }

    #endregion
}
