using System.ComponentModel.DataAnnotations;

namespace TheResistanceOnline.Web;

public class AppSettings
{
    #region Properties

    [Required]
    public string ClientUrl { get; set; }

    public AuthServer AuthServer { get; set; }

    #endregion
}

public class AuthServer
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

    #endregion
}
