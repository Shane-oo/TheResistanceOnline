using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheResistanceOnline.Data.ProfilePictures
{
    [Table("ProfilePictures")]
    public class ProfilePicture
    {
        #region Properties

        [Column("Description")]
        public string? Description { get; set; }

        [Column("Id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("Name")]
        public string? Name { get; set; }

        #endregion
    }
}
