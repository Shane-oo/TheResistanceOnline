using TheResistanceOnline.Core.NewCommandAndQueries;

namespace TheResistanceOnline.Authentications.ExternalIdentities;

public static class ExternalIdentityErrors
{
    #region Fields

    public static readonly Error MissingIdentifier = new("ExternalIdentity.MissingIdentifier",
                                                         "The required external identifiers were not provided");

    #endregion
}
