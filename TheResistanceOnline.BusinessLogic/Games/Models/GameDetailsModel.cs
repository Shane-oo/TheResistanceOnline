using JetBrains.Annotations;

namespace TheResistanceOnline.BusinessLogic.Games.Models
{
    public class GameDetailsModel
    {
        #region Properties

        public string ChannelName { get; set; }

        public bool IsVoiceChannel { get; set; }
        
        public bool IsAvailable { get; set; }

        [CanBeNull]
        public List<PlayerDetailsModel> PlayersDetails { get; set; }

        public bool UserInGame { get; set; }

        
        #endregion
    }
}
