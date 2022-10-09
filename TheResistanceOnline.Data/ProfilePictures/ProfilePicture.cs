using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;
using TheResistanceOnline.Data.Users;

namespace TheResistanceOnline.Data.ProfilePictures
{
    [Table("ProfilePictures")]
    public class ProfilePicture
    {
        #region Properties

        [Column("Description")]
        [CanBeNull]
        public string Description { get; set; }

        [Column("Id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("Name")]
        [CanBeNull]
        public string Name { get; set; }

        public User User { get; set; }

        public string UserId { get; set; }

        #endregion
    }
}
