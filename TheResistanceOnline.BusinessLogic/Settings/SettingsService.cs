using Microsoft.Extensions.Options;

namespace TheResistanceOnline.BusinessLogic.Settings;

public interface ISettingsService
{
    Models.Settings GetAppSettings();
}

public class SettingsService: ISettingsService
{
    #region Fields

    private readonly Models.Settings _settings;

    #endregion

    #region Construction

    public SettingsService(IOptions<Models.Settings> settings)
    {
        _settings = settings.Value;
    }

    #endregion

    #region Public Methods

    public Models.Settings GetAppSettings()
    {
        return new Models.Settings
               {
                   DiscordLoginToken = _settings.DiscordLoginToken,
                   JWTExpiryInMinutes = _settings.JWTExpiryInMinutes,
                   JWTValidIssuer = _settings.JWTValidIssuer,
                   JWTValidAudience = _settings.JWTValidAudience,
                   JWTSecurityKey = _settings.JWTSecurityKey,
                   SendGridClientToken = _settings.SendGridClientToken
               };
    }

    #endregion
}
