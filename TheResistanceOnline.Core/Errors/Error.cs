namespace TheResistanceOnline.Core.Errors;

public record Error(string Code, string Description)
{
    #region Fields

    public static readonly Error None = new(string.Empty, string.Empty);

    public static readonly Error NullValue = new("Error.NullValue", "Value cannot be null");

    public static readonly Error Unknown = new("Error", "Something went wrong");

    #endregion
}
