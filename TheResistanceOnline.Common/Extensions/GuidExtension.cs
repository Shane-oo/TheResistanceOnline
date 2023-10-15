namespace TheResistanceOnline.Common.Extensions;

public static class GuidExtension
{
    #region Public Methods

    public static bool HasValue(this Guid guid)
    {
        return guid != Guid.Empty;
    }

    #endregion
}
