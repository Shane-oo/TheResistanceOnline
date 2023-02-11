using System.Text.Json.Serialization;

namespace TheResistanceOnline.BusinessLogic.Games.Models
{
    public class PlayerDetailsModel
    {
        #region Properties

        public bool ApprovedMissionTeam { get; set; }

        // dont send to front end
        [JsonIgnore]
        public IGamePlayingBotObserver BotObserver { get; set; }

        public bool Chose { get; set; }

        public string ConnectionId { get; set; }

        public bool Continued { get; set; }

        public bool IsBot { get; set; }

        public bool IsInAGame { get; set; }

        public bool IsMissionLeader { get; set; }

        public Guid PlayerId { get; set; }

        public int ResistanceTeamWins { get; set; }

        public bool SelectedTeamMember { get; set; }

        public int SpyTeamWins { get; set; }

        public bool SupportedMission { get; set; }

        public TeamModel Team { get; set; }

        public string UserName { get; set; }

        public bool Voted { get; set; }

        #endregion
    }
}
