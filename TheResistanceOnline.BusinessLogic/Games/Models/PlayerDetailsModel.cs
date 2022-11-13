using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace TheResistanceOnline.BusinessLogic.Games.Models
{
    public class PlayerDetailsModel
    {
        #region Properties

        // dont send to front end
        [JsonIgnore]
        public IBotObserver BotObserver { get; set; }

        public string ConnectionId { get; set; }

        public string DiscordTag { get; set; }

        [CanBeNull]
        public string DiscordUserName { get; set; }

        public bool IsBot { get; set; }

        public bool IsInAGame { get; set; }

        public bool IsMissionLeader { get; set; }

        public Guid PlayerId { get; set; }

        public int ResistanceTeamWins { get; set; }

        public bool SelectedTeamMember { get; set; }

        public int SpyTeamWins { get; set; }

        public TeamModel Team { get; set; }

        public string UserName { get; set; }

        #endregion
    }
}
