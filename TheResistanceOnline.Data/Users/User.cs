using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace TheResistanceOnline.Data.Users;

public class User: IdentityUser
{
    #region Properties

    [ForeignKey("ProfilePicture")]
    [Column("ProfilePictureId")]
    public int? ProfilePictureId { get; set; }

    #endregion
}
