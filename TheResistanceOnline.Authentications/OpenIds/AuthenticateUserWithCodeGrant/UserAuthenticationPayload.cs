using TheResistanceOnline.Data.Entities;

namespace TheResistanceOnline.Authentications.OpenIds;

public class UserAuthenticationPayload
{
    #region Properties

    public string Role { get; init; }

    public UserId UserId { get; init; }

    public string UserName { get; init; }

    #endregion
}
