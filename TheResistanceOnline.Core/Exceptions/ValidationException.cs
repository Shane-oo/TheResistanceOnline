namespace TheResistanceOnline.Core.Exceptions;

public class ValidationException: Exception
{
    #region Properties

    public IEnumerable<ValidationError> Errors { get; }

    #endregion

    #region Construction

    public ValidationException(IEnumerable<ValidationError> errors)
    {
        Errors = errors;
    }

    #endregion
}
