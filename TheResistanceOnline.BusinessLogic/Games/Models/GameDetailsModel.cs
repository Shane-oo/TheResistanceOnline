using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace TheResistanceOnline.BusinessLogic.Games.Models
{
    public class GameDetailsModel
    {
        #region Properties

        public string ChannelName { get; set; }

        public int CurrentMissionRound { get; set; }

        public GameActionModel GameAction { get; set; }

        public GameOptionsModel GameOptions { get; set; }

        public GameStageModel GameStage { get; set; }

        public bool IsAvailable { get; set; }

        public bool IsFinished { get; set; }

        public bool IsVoiceChannel { get; set; }

        public List<bool> MissionOutcome { get; set; }

        public Dictionary<int, bool> MissionRounds { get; set; }

        public int MissionSize { get; set; }

        [CanBeNull]
        public List<PlayerDetailsModel> MissionTeam { get; set; }

        public bool MoveToNextRound { get; set; }

        public GameStageModel NextGameStage { get; set; }

        [CanBeNull]
        public List<PlayerDetailsModel> PlayersDetails { get; set; }

        [JsonIgnore]
        public ISpectatorBotObserver PlayerValuesSpectator { get; set; }

        public int VoteFailedCount { get; set; }

        #endregion
    }
}
