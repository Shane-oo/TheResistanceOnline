namespace TheResistanceOnline.Core.Errors;

public sealed record ValidationError(string PropertyName, string ErrorMessage);
