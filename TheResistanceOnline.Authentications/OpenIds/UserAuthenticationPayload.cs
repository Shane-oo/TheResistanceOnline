using TheResistanceOnline.Data.Entities;

namespace TheResistanceOnline.Authentications.OpenIds;

public class UserAuthenticationPayload
{
    #region Properties

    public string Role { get; set; }

    public UserId UserId { get; set; }

    public string UserName { get; set; }

    #endregion
}
