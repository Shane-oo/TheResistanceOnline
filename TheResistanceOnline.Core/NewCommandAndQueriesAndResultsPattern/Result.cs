namespace TheResistanceOnline.Core.NewCommandAndQueriesAndResultsPattern;

public class Result
{
    #region Properties

    public Error Error { get; }

    public bool IsFailure => !IsSuccess;

    public bool IsSuccess { get; }

    #endregion

    #region Construction

    protected Result(bool isSuccess, Error error)
    {
        switch(isSuccess)
        {
            case true when error != Error.None:
                throw new InvalidOperationException();
            case false when error == Error.None:
                throw new InvalidOperationException();
            default:
                IsSuccess = isSuccess;
                Error = error;
                break;
        }
    }

    #endregion

    #region Public Methods

    public static Result<TValue> Create<TValue>(TValue value)
    {
        return value != null ? Success(value) : Failure<TValue>(Error.NullValue);
    }

    public static Result Failure(Error error)
    {
        return new Result(false, error);
    }

    public static Result<TValue> Failure<TValue>(Error error)
    {
        return new Result<TValue>(default, false, error);
    }

    public static Result Success()
    {
        return new Result(true, Error.None);
    }

    public static Result<TValue> Success<TValue>(TValue value)
    {
        return new Result<TValue>(value, true, Error.None);
    }

    #endregion
}

public class Result<TValue>: Result
{
    #region Fields

    private readonly TValue _value;

    #endregion

    #region Properties

    public TValue Value => IsSuccess ? _value! : throw new InvalidOperationException("The value of a failure result can not be accessed.");

    #endregion

    #region Construction

    protected internal Result(TValue value, bool isSuccess, Error error): base(isSuccess, error)
    {
        _value = value;
    }

    #endregion

    #region Public Methods

    public static implicit operator Result<TValue>(TValue value)
    {
        return Create(value);
    }

    #endregion
}
