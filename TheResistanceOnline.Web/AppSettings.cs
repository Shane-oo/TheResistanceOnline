using System.ComponentModel.DataAnnotations;

namespace TheResistanceOnline.Web;

public class AppSettings
{
    #region Properties

    [Required]
    public string ClientUrl { get; set; }

    #endregion
}
