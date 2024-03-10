using TheResistanceOnline.Core.Errors;
using TheResistanceOnline.Core.Exchange.Responses;
using TheResistanceOnline.GamePlay.Common;
using TheResistanceOnline.GamePlay.ObserverPattern;
using TheResistanceOnline.GamePlay.PlayerModels;

namespace TheResistanceOnline.GamePlay.GameModels;

public abstract class GameModel: IGameModelSubject
{
    #region Fields

    private readonly List<IObserver> _observers = new();

    #endregion

    #region Properties

    public List<PlayerModel> Bots => Players.Where(p => p.Value.IsBot)
                                            .Select(kv => kv.Value)
                                            .ToList();

    public int Mission { get; private set; } = 1;

    public PlayerModel MissionLeader => Players.First(c => c.Value.IsMissionLeader).Value;

    public int MissionSize => GetMissionSize();

    public List<PlayerModel> MissionTeam { get; private set; } = new();

    public Phase Phase { get; private set; } = Phase.MissionBuild;

    public List<string> PlayerNames => Players.Keys.ToList();

    public Dictionary<string, PlayerModel> Players { get; } = new();

    public int TotalPlayers => Players.Count;

    public int VoteTrack { get; set; } = 1;

    public Team Winner { get; set; }

    public int SpyMissionWins { get; set; } = 0;

    public int ResistanceMissionWins { get; set; } = 0;

    #endregion

    #region Private Methods

    protected void CheckBotNeedsToPickMissionTeam()
    {
        var missionLeader = MissionLeader;

        if (missionLeader.IsBot)
        {
            missionLeader.BotModel.SelectAMissionTeam();
            SubmitMissionTeam();

            foreach(var bot in Bots)
            {
                bot.BotModel.VoteForMissionTeam();
            }
        }
    }


    protected static List<PlayerSetupModel> CreatePlayerSetupModels(List<string> userNames, int botCount)
    {
        var playerSetupModels = new List<PlayerSetupModel>();
        foreach(var playerUserName in userNames)
        {
            playerSetupModels.Add(new PlayerSetupModel(playerUserName, false));
        }

        for(var i = 0; i < botCount; i++)
        {
            playerSetupModels.Add(new PlayerSetupModel($"Bot-{i}", true));
        }

        return playerSetupModels;
    }

    protected void MoveMissionLeaderClockwise()
    {
        var playerList = Players.Values.ToList();

        var missionLeader = playerList.FirstOrDefault(c => c.IsMissionLeader);
        if (missionLeader != null)
        {
            missionLeader.IsMissionLeader = false;

            var index = playerList.IndexOf(missionLeader);

            // was the last person in array
            if (index == playerList.Count)
            {
                playerList[0].IsMissionLeader = true;
            }
            else
            {
                playerList[index + 1].IsMissionLeader = true;
            }
        }
        else
        {
            // Mission Leader has not been set yet
            playerList[0].IsMissionLeader = true;
        }

        NotifyObservers();
    }


    protected void UpdateMission(int mission)
    {
        Mission = mission;
        NotifyObservers();
    }

    private void CheckBotsNeedToDecideMissionOutcome()
    {
        var missionBots = Players.Values.Where(p => MissionTeam.Contains(p) && p.IsBot);

        foreach(var missionBot in missionBots)
        {
            missionBot.BotModel.DecideMissionOutcome();
        }
    }

    private void ClearMissionTeam()
    {
        MissionTeam = [];
    }

    private void EndGame(Team winner)
    {
        UpdatePhase(Phase.GameOver);
        Winner = winner;
    }


    private int GetMissionSize()
    {
        var missionSize = 0;
        switch(TotalPlayers)
        {
            case 5:
                switch(Mission)
                {
                    case 1:
                    case 3:
                        missionSize = 2;
                        break;
                    case 2:
                    case 4:
                    case 5:
                        missionSize = 3;
                        break;
                }

                break;
            case 6:
                switch(Mission)
                {
                    case 1:
                        missionSize = 2;
                        break;
                    case 2:
                    case 4:
                        missionSize = 3;
                        break;
                    case 3:
                    case 5:
                        missionSize = 4;
                        break;
                }

                break;
            case 7:
                switch(Mission)
                {
                    case 1:
                        missionSize = 2;
                        break;
                    case 2:
                    case 3:
                        missionSize = 3;
                        break;
                    case 4:
                    case 5:
                        missionSize = 4;
                        break;
                }

                break;
            case 8:
            case 9:
            case 10:
                switch(Mission)
                {
                    case 1:
                        missionSize = 3;
                        break;
                    case 2:
                    case 3:
                        missionSize = 4;
                        break;
                    case 4:
                    case 5:
                        missionSize = 5;
                        break;
                }


                break;
        }

        return missionSize;
    }

    private bool GetMissionSuccessful()
    {
        // in games of 7 or more, mission 4 requires two fail cards
        var missionFailedCardsRequired = Players.Count >= 7 && Mission == 4 ? 2 : 1;

        var missionFailed = MissionTeam.Count(p => p.MissionChoice == false) >= missionFailedCardsRequired;

        return !missionFailed;
    }

    private bool GetVoteSuccessful()
    {
        // note: A tied vote is a fail
        return Players.Values.Count(p => p.VoteChoice == true) > Players.Count / 2;
    }

    private bool IncrementVoteTrack()
    {
        VoteTrack++;
        if (VoteTrack == 6)
        {
            // Spies automatically win if 5 failed votes happen in a row
            EndGame(Team.Spy);
            return true;
        }

        NotifyObservers();
        return false;
    }

    private bool MissionOver(bool missionSuccessful)
    {
        Mission++;
        if (missionSuccessful)
        {
            ResistanceMissionWins++;
        }
        else
        {
            SpyMissionWins++;
        }

        if (ResistanceMissionWins != 3 && SpyMissionWins != 3)
        {
            return false;
        }

        var winningTeam = ResistanceMissionWins == 3 ? Team.Resistance : Team.Spy;
        EndGame(winningTeam);

        return true;
    }

    private void ResetPlayerMissionChoices()
    {
        foreach(var player in Players.Values)
        {
            player.MissionChoice = null;
        }
    }

    private void ResetPlayerVotes()
    {
        foreach(var player in Players.Values)
        {
            player.VoteChoice = null;
        }
    }

    private void ResetVoteTrack()
    {
        VoteTrack = 1;
    }

    private void UpdatePhase(Phase phase)
    {
        Phase = phase;
        NotifyObservers();
    }

    #endregion

    #region Public Methods

    public void AddMissionTeamMember(string player)
    {
        ArgumentNullException.ThrowIfNull(player);

        if (MissionTeamFull())
        {
            return;
        }

        var playerModel = Players.Values.FirstOrDefault(p => p.Name == player);

        MissionTeam.Add(playerModel);

        NotifyObservers();
    }


    public Result<MissionResultsModel> GetMissionResults()
    {
        // Check everybody has submitted mission results
        if (MissionTeam.Any(p => p.MissionChoice == null))
        {
            return Result.Failure<MissionResultsModel>(new Error("Game.Error", "Waiting for more mission outcomes"));
        }

        var missionSuccessChoices = MissionTeam.Count(p => p.MissionChoice == true);
        var missionFailureChoices = MissionTeam.Count(p => p.MissionChoice == false);
        var missionSuccessful = GetMissionSuccessful();

        var gameOver = MissionOver(missionSuccessful);

        if (!gameOver)
        {
            // Cleanup
            ResetPlayerMissionChoices();
            ClearMissionTeam();

            UpdatePhase(Phase.MissionBuild);
            MoveMissionLeaderClockwise();
            CheckBotNeedsToPickMissionTeam();
        }

        return Result.Success(new MissionResultsModel(missionSuccessChoices, missionFailureChoices, missionSuccessful));
    }

    public IEnumerable<PlayerModel> GetMissionTeam()
    {
        return MissionTeam;
    }

    public PlayerModel GetPlayerModel(string name)
    {
        return Players.FirstOrDefault(p => p.Key == name).Value;
    }

    public Result<VoteResultsModel> GetVoteResults()
    {
        // Check everybody has voted
        if (Players.Any(p => p.Value.VoteChoice == null))
        {
            return Result.Failure<VoteResultsModel>(new Error("Game.Error", "Waiting for more votes"));
        }

        var playerNameToVoteApproved = Players.ToDictionary(player => player.Key, player => player.Value.VoteChoice == true);
        var voteSuccessful = GetVoteSuccessful();
        ResetPlayerVotes();

        if (voteSuccessful)
        {
            UpdatePhase(Phase.Mission);
            ResetVoteTrack();
            CheckBotsNeedToDecideMissionOutcome();
        }
        else
        {
            var gameOver = IncrementVoteTrack();
            if (!gameOver)
            {
                ClearMissionTeam();
                UpdatePhase(Phase.MissionBuild);
                MoveMissionLeaderClockwise();
                CheckBotNeedsToPickMissionTeam();
            }
        }

        return new VoteResultsModel(playerNameToVoteApproved,
                                    voteSuccessful);
    }

    public bool MissionTeamFull()
    {
        return MissionTeam.Count == MissionSize;
    }

    public void NotifyObservers()
    {
        foreach(var observer in _observers)
        {
            observer.Update();
        }
    }

    public void RegisterObserver(IObserver observer)
    {
        _observers.Add(observer);
    }

    public void RemoveMissionTeamMember(string player)
    {
        ArgumentNullException.ThrowIfNull(player);

        var playerModel = Players.Values.FirstOrDefault(p => p.Name == player);

        MissionTeam.Remove(playerModel);

        NotifyObservers();
    }

    public void RemoveObserver(IObserver observer)
    {
        _observers.Remove(observer);
    }

    public abstract void SetupGame(List<string> playerUserNames, int botCount);

    public Result SubmitMissionTeam()
    {
        if (!MissionTeamFull())
        {
            return Result.Failure(new Error("Game.Error", "Mission team is not full "));
        }

        UpdatePhase(Phase.Vote);
        return Result.Success();
    }

    #endregion
}
