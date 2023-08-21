using System.ComponentModel.DataAnnotations;

namespace TheResistanceOnline.Core;

public class AppSettings
{
    #region Properties

    public AuthServerSettings AuthServerSettings { get; set; }

    [Required]
    public string ClientUrl { get; set; }

    #endregion
}

public class AuthServerSettings
{
    #region Properties

#if RELEASE
    [Required]
#endif
    public string EncryptionCertificateThumbprint { get; set; }

#if RELEASE
    [Required]
#endif
    public string SigningCertificateThumbprint { get; set; }

    [Required]
    public MicrosoftSettings MicrosoftSettings { get; set; }

    [Required]
    public GoogleSettings GoogleSettings { get; set; }

    #endregion
}

public class MicrosoftSettings
{
    #region Properties

    [Required]
    public string ClientId { get; set; }

    [Required]
    public string ClientSecret { get; set; }

    [Required]
    public string RedirectUri { get; set; }

    #endregion
}

public class GoogleSettings
{
    [Required]
    public string ClientId { get; set; }

    [Required]
    public string ClientSecret { get; set; }

    [Required]
    public string RedirectUri { get; set; }
}
