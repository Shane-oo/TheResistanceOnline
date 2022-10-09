using JetBrains.Annotations;

namespace TheResistanceOnline.BusinessLogic.Games.Models
{
    public class PlayerDetailsModel
    {
        #region Properties

        public string DiscordTag { get; set; }

        [CanBeNull]
        public string DiscordUserName { get; set; }

        public bool IsInAGame { get; set; }

        public Guid PlayerId { get; set; }

        public int ResistanceTeamWins { get; set; }

        public int SpyTeamWins { get; set; }

        public string UserName { get; set; }

        #endregion
    }
}
