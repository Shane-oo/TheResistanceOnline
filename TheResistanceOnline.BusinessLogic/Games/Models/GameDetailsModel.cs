using JetBrains.Annotations;

namespace TheResistanceOnline.BusinessLogic.Games.Models
{
    public class GameDetailsModel
    {
        #region Properties

        public string ChannelName { get; set; }

        public GameActionModel GameAction { get; set; }

        public GameOptionsModel GameOptions { get; set; }

        public GameStageModel GameStage { get; set; }
        
        public GameStageModel NextGameStage { get; set; }

        public bool IsAvailable { get; set; }

        public bool IsFinished { get; set; }

        public bool IsVoiceChannel { get; set; }

        public int MissionRound { get; set; }

        public int MissionSize { get; set; }

        [CanBeNull]
        public List<PlayerDetailsModel> MissionTeam { get; set; }

        [CanBeNull]
        public List<PlayerDetailsModel> PlayersDetails { get; set; }

        #endregion
    }
}
