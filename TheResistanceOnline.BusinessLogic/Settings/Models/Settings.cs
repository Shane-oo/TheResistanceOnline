namespace TheResistanceOnline.BusinessLogic.Settings.Models;

public class Settings
{
    #region Properties
    
    public string JWTExpiryInMinutes { get; set; }

    public string JWTSecurityKey { get; set; }

    public string JWTValidAudience { get; set; }

    public string JWTValidIssuer { get; set; }

    public string ResistanceDbConnectionString { get; set; }

    public string SendGridClientToken { get; set; }

    #endregion
}
