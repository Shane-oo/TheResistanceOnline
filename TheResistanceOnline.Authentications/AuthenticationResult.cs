namespace TheResistanceOnline.Authentications;

public class AuthenticationResult
{
    #region Properties

    public bool IsAuthenticated { get; set; }

    public string Reason { get; set; }

    #endregion

    #region Public Methods

    public static AuthenticationResult Accept()
    {
        return new AuthenticationResult { IsAuthenticated = true };
    }

    public static AuthenticationResult Reject(string reason)
    {
        return new AuthenticationResult { IsAuthenticated = false, Reason = reason };
    }

    #endregion
}

public class AuthenticationResult<T>: AuthenticationResult
{
    #region Properties

    public T Payload { get; set; }

    #endregion

    #region Public Methods

    public static AuthenticationResult<T> Accept(T payload)
    {
        return new AuthenticationResult<T> { IsAuthenticated = true, Payload = payload };
    }

    public new static AuthenticationResult<T> Reject(string reason)
    {
        return new AuthenticationResult<T> { IsAuthenticated = false, Reason = reason };
    }

    #endregion
}
