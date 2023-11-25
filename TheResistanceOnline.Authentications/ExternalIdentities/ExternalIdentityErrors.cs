using TheResistanceOnline.Core.NewCommandAndQueriesAndResultsPattern;

namespace TheResistanceOnline.Authentications.ExternalIdentities;

public static class ExternalIdentityErrors
{
    #region Fields

    public static readonly Error MissingIdentifier = new("ExternalIdentity.MissingIdentifier",
                                                         "The required external identifier was not provided");

    #endregion
}
