using TheResistanceOnline.BusinessLogic.Games.BotObservers;
using TheResistanceOnline.BusinessLogic.Games.Models;

namespace TheResistanceOnline.BusinessLogic.Games
{
    public interface IGameService
    {
        void MoveToNextRound(GameDetailsModel gameDetails);

        void ProcessMission(GameDetailsModel gameDetails);

        void ProcessMissionPropose(GameDetailsModel gameDetails);

        void ProcessVote(GameDetailsModel gameDetails);

        void ProcessContinue(GameDetailsModel gameDetails);

        GameDetailsModel SetUpNewGame(GameDetailsModel gameDetails);
    }

    public class GameService: IGameService
    {
        #region Constants

        // const for game sizes
        private const int EIGHT_MAN_GAME = 8;
        private const int FIVE_MAN_GAME = 5;
        private const int NINE_MAN_GAME = 9;
        private const int SEVEN_MAN_GAME = 7;
        private const int SIX_MAN_GAME = 6;
        private const int TEN_MAN_GAME = 10;

        #endregion

        #region Fields

        private static readonly Random _random = new();
        

        #endregion

        #region Private Methods

        private static List<PlayerDetailsModel> GetRandomPlayers(int amount, IReadOnlyList<PlayerDetailsModel> playerList)
        {
            var desiredList = new List<PlayerDetailsModel>();
            var count = 0;
            while(count != amount)
            {
                var randomPlayer = playerList[_random.Next(playerList.Count)];

                // check for if randomly got the same person again
                if (desiredList.Any(p => p.PlayerId == randomPlayer.PlayerId)) continue;

                desiredList.Add(randomPlayer);
                count++;
            }

            return desiredList;
        }

        private static bool MoveMissionLeaderClockwise(GameDetailsModel gameDetails)
        {
            if (gameDetails.PlayersDetails != null)
            {
                for(var i = 0; i <= gameDetails.PlayersDetails.Count; i++)
                {
                    if (gameDetails.PlayersDetails[i].IsMissionLeader)
                    {
                        gameDetails.PlayersDetails[i].IsMissionLeader = false;
                        if (i == gameDetails.PlayersDetails?.Count - 1)
                        {
                            gameDetails.PlayersDetails![0].IsMissionLeader = true;
                            return gameDetails.PlayersDetails[0].IsBot;
                        }

                        gameDetails.PlayersDetails![i + 1].IsMissionLeader = true;
                        return gameDetails.PlayersDetails[i + 1].IsBot;
                    }
                }
            }

            return false;
        }

        private static List<bool> ShuffleMissionOutcomes(IEnumerable<bool> missionOutcomes)
        {
            return missionOutcomes.OrderBy(mo => _random.Next()).ToList();
        }

        #endregion

        #region Public Methods

        public int GetMissionSize(int missionRound, int playerCount)
        {
            switch(playerCount)
            {
                case FIVE_MAN_GAME:
                    switch(missionRound)
                    {
                        case 1:
                        case 3:
                            return 2;
                        case 2:
                        case 4:
                        case 5:
                            return 3;
                    }

                    break;
                case SIX_MAN_GAME:
                    switch(missionRound)
                    {
                        case 1:
                            return 2;
                        case 2:
                        case 4:
                            return 3;
                        case 3:
                        case 5:
                            return 4;
                    }

                    break;
                case SEVEN_MAN_GAME:
                    switch(missionRound)
                    {
                        case 1:
                            return 2;
                        case 2:
                        case 3:
                            return 3;
                        case 4:
                        case 5:
                            return 4;
                    }

                    break;
                case EIGHT_MAN_GAME:
                case NINE_MAN_GAME:
                case TEN_MAN_GAME:
                    // 8,9,10 man teams are the same
                    switch(missionRound)
                    {
                        case 1:
                            return 3;
                        case 2:
                        case 3:
                            return 4;
                        case 4:
                        case 5:
                            return 5;
                    }

                    break;
            }

            return -1;
        }



        public void MoveToNextRound(GameDetailsModel gameDetails)
        {
            gameDetails.CurrentMissionRound++;
            gameDetails.MissionSize = GetMissionSize(gameDetails.CurrentMissionRound, gameDetails.PlayersDetails!.Count);
        }


        public void ProcessMission(GameDetailsModel gameDetails)
        {
            var missionMembers = gameDetails.PlayersDetails!.Where(p2 => gameDetails.MissionTeam!.Any(p1 => p1.PlayerId == p2.PlayerId)).ToList();

            var playerMissionMembers = missionMembers.Where(p => !p.IsBot).ToList();

            if (playerMissionMembers.All(p => p.Chose))
            {
                var missionBots = missionMembers.Where(p => p.IsBot).ToList();

                foreach(var bot in missionBots)
                {
                    bot.Chose = true;
                    bot.SupportedMission = bot.BotObserver.GetMissionChoice();
                }

                gameDetails.MissionOutcome = ShuffleMissionOutcomes(missionMembers.Select(p => p.SupportedMission));

                //the 4th Mission in games of 7 or more players require at least two mission fail cards to be a failed mission
                if (gameDetails.PlayersDetails!.Count > 6 && gameDetails.CurrentMissionRound == 4)
                {
                    gameDetails.MissionRounds.Add(gameDetails.CurrentMissionRound, gameDetails.MissionOutcome.Count(mo => !mo) <= 1);
                }
                else
                {
                    gameDetails.MissionRounds.Add(gameDetails.CurrentMissionRound, gameDetails.MissionOutcome.All(mo => mo));
                }

                // Check If a Team has Won the game by winning 3 rounds
                var resistanceRoundWins = 0;
                var spyRoundWins = 0;
                foreach(var missionRound in gameDetails.MissionRounds)
                {
                    if (missionRound.Value == false)
                    {
                        spyRoundWins++;
                    }
                    else
                    {
                        resistanceRoundWins++;
                    }
                }

                if (resistanceRoundWins == 3)
                {
                    gameDetails.GameStage = GameStageModel.GameOverResistanceWon;
                }
                else if (spyRoundWins == 3)
                {
                    gameDetails.GameStage = GameStageModel.GameOverSpiesWon;
                }
                else
                {
                    gameDetails.GameStage = GameStageModel.MissionResults;
                    gameDetails.NextGameStage = GameStageModel.MissionPropose;
                    gameDetails.MoveToNextRound = true;
                }
            }
        }

        public void ProcessMissionPropose(GameDetailsModel gameDetails)
        {
            if (gameDetails.MoveToNextRound)
            {
                MoveToNextRound(gameDetails);
            }

            // Empty Mission Team
            gameDetails.MissionTeam = new List<PlayerDetailsModel>();
            var newMissionLeaderIsBot = MoveMissionLeaderClockwise(gameDetails);
            if (newMissionLeaderIsBot)
            {
                var leaderBot = gameDetails.PlayersDetails?.FirstOrDefault(p => p.IsMissionLeader);
                gameDetails.MissionTeam = leaderBot?.BotObserver.GetMissionProposal();
                // skip Mission Propose on client side as bot has decided
                gameDetails.GameStage = GameStageModel.Vote;
            }
        }

        public void ProcessVote(GameDetailsModel gameDetails)
        {
            var players = gameDetails.PlayersDetails?.Where(p => !p.IsBot);
            if (players != null && players.All(p => p.Voted))
            {
                var bots = gameDetails.PlayersDetails?.Where(p => p.IsBot);
                if (bots != null)
                {
                    foreach(var bot in bots)
                    {
                        bot.Voted = true;
                        bot.ApprovedMissionTeam = bot.BotObserver.GetVote();
                    }
                }

                var votes = gameDetails.PlayersDetails?.Select(p => p.ApprovedMissionTeam).ToList();
                var approvedVotes = votes?.Where(v => v).Count();
                var rejectedVotes = votes?.Where(v => !v).Count();
                // successful vote => move onto mission game stage
                if (approvedVotes > rejectedVotes)
                {
                    gameDetails.VoteFailedCount = 0;
                    gameDetails.NextGameStage = GameStageModel.Mission;

                    //todo I need to do something on here about if all bots go on a mission!!!
                }
                else
                {
                    gameDetails.VoteFailedCount++;
                    gameDetails.NextGameStage = GameStageModel.MissionPropose;
                    gameDetails.MoveToNextRound = false;
                }

                // 5 consecutive failed votes => spies automatically win
                gameDetails.GameStage = gameDetails.VoteFailedCount == 5 ? GameStageModel.GameOverSpiesWon : GameStageModel.VoteResults;
            }
        }

        public void ProcessContinue(GameDetailsModel gameDetails)
        {
            var players = gameDetails.PlayersDetails?.Where(p => !p.IsBot);

            if (players != null && players.All(p => p.Continued))
            {
                //Reset Voted And Continued
                foreach(var playerDetail in gameDetails.PlayersDetails!)
                {
                    playerDetail.Voted = false;
                    playerDetail.Continued = false;
                    playerDetail.Chose = false;
                }

                gameDetails.GameStage = gameDetails.NextGameStage;
                switch(gameDetails.GameStage)
                {
                    case GameStageModel.Mission:
                        if (gameDetails.MissionTeam != null && gameDetails.MissionTeam.All(p => p.IsBot))
                        {
                            ProcessMission(gameDetails);
                        }
                        // Skip mission On client Side as bots complete mission

                        break;
                    case GameStageModel.MissionPropose:
                        ProcessMissionPropose(gameDetails);


                        break;
                }
            }
        }


        public GameDetailsModel SetUpNewGame(GameDetailsModel gameDetails)
        {
            if (gameDetails.PlayersDetails != null)
            {
                var shuffledPlayerDetails = gameDetails.PlayersDetails.OrderBy(p => _random.Next()).ToList();

                var spyPlayers = new List<PlayerDetailsModel>();
                switch(shuffledPlayerDetails.Count)
                {
                    case FIVE_MAN_GAME:
                    case SIX_MAN_GAME:
                        spyPlayers = GetRandomPlayers(2, shuffledPlayerDetails);
                        break;
                    case SEVEN_MAN_GAME:
                    case EIGHT_MAN_GAME:
                    case NINE_MAN_GAME:
                        spyPlayers = GetRandomPlayers(3, shuffledPlayerDetails);
                        break;
                    case TEN_MAN_GAME:
                        spyPlayers = GetRandomPlayers(4, shuffledPlayerDetails);
                        break;
                }

                // check each player to see if on spy team else assign to resistance team 
                foreach(var player in shuffledPlayerDetails)
                {
                    player.Team = spyPlayers.Any(p => p.PlayerId == player.PlayerId) ? TeamModel.Spy : TeamModel.Resistance;
                }

                // denote first player as mission leader
                gameDetails.PlayersDetails = shuffledPlayerDetails;
                gameDetails.PlayersDetails.First().IsMissionLeader = true;
            }

            gameDetails.MissionTeam = new List<PlayerDetailsModel>();
            gameDetails.CurrentMissionRound = 1;
            gameDetails.MissionRounds = new Dictionary<int, bool>();
            gameDetails.MissionSize = GetMissionSize(gameDetails.CurrentMissionRound, gameDetails.PlayersDetails!.Count);
            gameDetails.VoteFailedCount = 0;
            gameDetails.GameStage = GameStageModel.MissionPropose;
            
            
            
            return gameDetails;
        }

        #endregion
    }
}
