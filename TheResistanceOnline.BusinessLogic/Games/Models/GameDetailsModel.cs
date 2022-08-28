namespace TheResistanceOnline.BusinessLogic.Games.Models
{
    public class GameDetailsModel
    {
        #region Properties

        public string? LobbyName { get; set; }

        public List<PlayerDetailsModel>? PlayersDetails { get; set; }

        public bool UserInGame { get; set; }

        #endregion
    }
}
