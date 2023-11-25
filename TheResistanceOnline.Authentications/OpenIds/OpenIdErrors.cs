using TheResistanceOnline.Core.NewCommandAndQueriesAndResultsPattern;

namespace TheResistanceOnline.Authentications.OpenIds;

public static class OpenIdErrors
{
    #region Fields

    public static readonly Error UserNotFound = new("OpenId.UserNotFound",
                                                    "User was not found");

    #endregion
}
