namespace TheResistanceOnline.Core.Exceptions;

public sealed record ValidationError(string PropertyName, string ErrorMessage);
