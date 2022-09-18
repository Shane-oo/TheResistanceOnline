using JetBrains.Annotations;

namespace TheResistanceOnline.BusinessLogic.Games.Models
{
    public class PlayerDetailsModel
    {
        #region Properties

        public Guid PlayerId { get; set; }
       
        public int? ProfilePictureId { get; set; }

        [CanBeNull]
        public string UserName { get; set; }

        #endregion
    }
}
