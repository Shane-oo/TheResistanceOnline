namespace TheResistanceOnline.Authentications.OpenIds;

public class UserAuthenticationPayload
{
    #region Properties

    public string Role { get; set; }

    public Guid UserId { get; set; }

    public string UserName { get; set; }

    #endregion
}
