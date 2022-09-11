using JetBrains.Annotations;

namespace TheResistanceOnline.BusinessLogic.Games.Models
{
    public class GameDetailsModel
    {
        #region Properties

        [NotNull]
        public string LobbyName { get; set; }

        [CanBeNull]
        public List<PlayerDetailsModel> PlayersDetails { get; set; }

        public bool UserInGame { get; set; }

        #endregion
    }
}
