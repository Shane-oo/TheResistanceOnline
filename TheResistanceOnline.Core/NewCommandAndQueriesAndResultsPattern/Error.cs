namespace TheResistanceOnline.Core.NewCommandAndQueries;

public record Error(string Code, string Name)
{
    #region Fields

    public static readonly Error None = new Error(string.Empty, string.Empty);

    public static readonly Error NullValue = new Error("Error.NullValue", "Null value was provided");

    #endregion
}
