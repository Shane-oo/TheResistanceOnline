using JetBrains.Annotations;
using TheResistanceOnline.BusinessLogic.BotObservers.BayesAgent.Models;
using TheResistanceOnline.BusinessLogic.Games.Models;

namespace TheResistanceOnline.BusinessLogic.BotObservers.BayesAgent;

public class BayesBotObserver: IBotObserver, IGamePlayingBotObserver
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

    private bool _beginningOfGame = true;

    private GameDetailsModel _gameDetails = new();
    private readonly List<Guid> _outedSpies = new();

    private Dictionary<Guid, PlayerVariablesModel> _playerIdToGameData;

    // true == spy
    private Dictionary<Guid, bool> _playerIdToSpyPredictions;
    private static readonly Random _random = new();

    private readonly List<string> _randomBotNames = new()
                                                    {
                                                        "WALL - E", "R2D2", "K9", "Optimus Prime", "Rosie", "Bender", "C-3PO", "HAL 9000", "Data", "ASIMO", "The Terminator",
                                                        "Micro", "EVA", "RAM", "Sputnik", "Humanoid", "Chip", "Robo", "Robocop", "Alpha", "Beta", "Gamma", "Siri",
                                                        "Raspberry Pie", "AstroBoy", "Chappie", "Ultron", "Omega", "Hydra", "Pixels", "Shane", "Brandon", "Hamish", "Chloe"
                                                    };

    private readonly BayesTrainingDataModel _trainingData;

    #endregion

    #region Properties

    public Guid PlayerId { get; set; }


    private string Name { get; set; }


    private TeamModel Team { get; set; }

    #endregion

    #region Construction

    public BayesBotObserver(BayesTrainingDataModel trainingData)
    {
        _trainingData = trainingData;
        SetName();
    }

    #endregion

    #region Private Methods

    private int GetHowManySpiesInGame()
    {
        var spies = 0;
        switch(_gameDetails.PlayersDetails!.Count)
        {
            case FIVE_MAN_GAME:
            case SIX_MAN_GAME:
                spies = 2;
                break;
            case SEVEN_MAN_GAME:
            case EIGHT_MAN_GAME:
            case NINE_MAN_GAME:
                spies = 3;
                break;

            case TEN_MAN_GAME:
                spies = 4;
                break;
                ;
        }

        return spies;
    }

    [CanBeNull]
    private PlayerDetailsModel GetMissionLeader()
    {
        return _gameDetails.PlayersDetails?.FirstOrDefault(p => p.IsMissionLeader);
    }

    private static int GetMissionSize(int missionRound, int playerCount)
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

    private PlayerDetailsModel GetPlayerDetails()
    {
        return _gameDetails.PlayersDetails?.FirstOrDefault(p => p.PlayerId == PlayerId);
    }

    private PlayerVariablesModel GetPlayerVariables(Guid playerId)
    {
        _playerIdToGameData.TryGetValue(playerId, out var playerVariables);
        return playerVariables;
    }

    private List<PlayerDetailsModel> GetRandomPlayers(int amount)
    {
        var desiredList = new List<PlayerDetailsModel>();
        var count = 0;
        while(count != amount)
        {
            var randomPlayer = _gameDetails.PlayersDetails![_random.Next(_gameDetails.PlayersDetails.Count)];
            if (randomPlayer.PlayerId == PlayerId)
            {
                continue;
            }

            // check for if randomly got the same person again
            // And Not Yourself
            if (desiredList.Any(p => p.PlayerId == randomPlayer.PlayerId))
            {
                continue;
            }

            desiredList.Add(randomPlayer);
            count++;
        }

        return desiredList;
    }

    // Spy Helper
    private List<PlayerDetailsModel> GetRandomResistanceTeamPlayers(int amount)
    {
        var desiredList = new List<PlayerDetailsModel>();
        var count = 0;
        while(count != amount)
        {
            var randomPlayer = _gameDetails.PlayersDetails![_random.Next(_gameDetails.PlayersDetails.Count)];
            if (randomPlayer.PlayerId == PlayerId)
            {
                continue;
            }

            // check for if randomly got the same person again
            // check for if random player is spy => resistance members only
            if (desiredList.Any(p => p.PlayerId == randomPlayer.PlayerId)
                || randomPlayer.Team == TeamModel.Spy)
            {
                continue;
            }

            desiredList.Add(randomPlayer);
            count++;
        }

        return desiredList;
    }

    // Spy Helper
    private List<PlayerDetailsModel> GetRandomSpyTeamPlayers(int amount)
    {
        var desiredList = new List<PlayerDetailsModel>();
        var count = 0;
        while(count != amount)
        {
            var randomPlayer = _gameDetails.PlayersDetails![_random.Next(_gameDetails.PlayersDetails.Count)];
            if (randomPlayer.PlayerId == PlayerId)
            {
                continue;
            }

            // check for if randomly got the same person again
            // check for if random player is resistance => spy members only
            if (desiredList.Any(p => p.PlayerId == randomPlayer.PlayerId)
                || randomPlayer.Team == TeamModel.Resistance)
            {
                continue;
            }

            desiredList.Add(randomPlayer);
            count++;
        }

        return desiredList;
    }

    private bool GetResistanceVote(PlayerDetailsModel missionLeader)
    {
        // Avoid Failing Game
        // Mission 1 always Yes
        if (_gameDetails.VoteFailedCount == 4 || _gameDetails.CurrentMissionRound == 1)
        {
            return true;
        }

        var containsOutedSpy = _gameDetails.MissionTeam!.Any(p => _outedSpies.Any(os => os == p.PlayerId));
        if (containsOutedSpy)
        {
            return false;
        }

        // Use Predictions to decide vote, if it is not null and a spy prediction has been calculated
        if (_playerIdToSpyPredictions != null && _playerIdToSpyPredictions.ContainsValue(true))
        {
            var containsPredictedSpies = _gameDetails.MissionTeam!.Any(p => _playerIdToSpyPredictions.Any(sp => sp.Key == p.PlayerId && sp.Value));
            return !containsPredictedSpies;
        }

        // Vote yes for a team that has all successfully passed missions
        var allBeenOnSuccessfulMissionsOrYetToBeOnMission =
            _gameDetails.MissionTeam!.Any(p => _playerIdToGameData.All(gd => (gd.Key == p.PlayerId && gd.Value.WentOnSuccessfulMission != 0 && gd.Value.WentOnFailedMission == 0) ||
                                                                             (gd.Value.WentOnFailedMission == 0 && gd.Value.WentOnSuccessfulMission == 0)));
        if (allBeenOnSuccessfulMissionsOrYetToBeOnMission)
        {
            return true;
        }

        // Vote no if any have been on failed missions
        var aMemberHasBeenOnAFailedMission = _gameDetails.MissionTeam!.Any(p => _playerIdToGameData.Any(gd => gd.Key == p.PlayerId && gd.Value.WentOnFailedMission != 0));
        if (aMemberHasBeenOnAFailedMission)
        {
            return false;
        }

        return _gameDetails.MissionTeam!.Any(p => p.PlayerId == PlayerId);
    }

    //The method should return false if this agent chooses to betray the mission, and true otherwise. 
    private bool GetSpyBetrayal()
    {
        var spyCount = _gameDetails.MissionTeam!.Count(p => p.Team == TeamModel.Spy);
        switch(_gameDetails.CurrentMissionRound)
        {
            case 1:
                // Dont Betray first mission always
                return true;
            case 5:
                // Always Betray last mission
                return false;
        }

        if (_gameDetails.CurrentMissionRound != 4)
        {
            return spyCount switch
            {
                1 => false,
                2 => _random.Next(0, 100) >= 75,
                _ => _random.Next(0, 100) >= 50
            };
        }

        // Mission 4 Scenario
        var spyWins = _gameDetails.MissionRounds.Values.Count(mo => !mo);
        if (spyWins == 1)
        {
            switch(_gameDetails.PlayersDetails!.Count)
            {
                case >= SEVEN_MAN_GAME when spyCount < 2:
                    // Require Mission With More than 2 Spies
                    return true;
                case < SEVEN_MAN_GAME when spyCount == 1:
                    // Only need 1 Betray
                    return false;
                case >= SEVEN_MAN_GAME when spyCount == 2:
                    // Need both spies to betray
                    return false;
                case >= SEVEN_MAN_GAME:

                    return _random.Next(0, 100) >= 50;
            }
        }

        // Dont care about mission 4 will go for mission 5 win
        return true;
    }

    private bool GetSpyVote(PlayerDetailsModel missionLeader)
    {
        var spyCount = _gameDetails.MissionTeam!.Count(p => p.Team == TeamModel.Spy);
        if (_gameDetails.CurrentMissionRound == 1)
        {
            return true;
        }

        if (_gameDetails.CurrentMissionRound != 4)
        {
            // Approve missions that have spies on it but not if mission if full of spies
            return _gameDetails.MissionSize != spyCount && spyCount > 0;
        }

        var spyWins = _gameDetails.MissionRounds.Values.Count(mo => !mo);

        if (spyWins != 1)
        {
            // Dont care about winning mission 4 will go for mission 5 win
            return true;
        }

        // Need To Win Mission 4 To Win Game
        switch(_gameDetails.PlayersDetails!.Count)
        {
            case >= SEVEN_MAN_GAME when spyCount < 2:
                // Require Mission With More than 2 Spies
                return false;
            case < SEVEN_MAN_GAME when spyCount > 0:
                return true;
            default:
                // Too many spies or no spies at all
                return false;
        }
    }

    // Check for spies that have completely outed themselves on failed missions
    private void OutedSpiesCheck(PlayerDetailsModel missionPlayer, int betrayals)
    {
        // spy outed themself on mission with me
        if (_gameDetails.MissionTeam!.Any(p => p.PlayerId == PlayerId))
        {
            switch(_gameDetails.MissionSize)
            {
                case 2 when betrayals == 1:
                case 3 when betrayals == 2:
                case 4 when betrayals == 3:
                case 5 when betrayals == 4:
                    _outedSpies.Add(missionPlayer.PlayerId);
                    break;
            }
        }
        // Spies Outed Eachother
        else
        {
            switch(_gameDetails.MissionSize)
            {
                case 2 when betrayals == 2:
                case 3 when betrayals == 3:
                case 4 when betrayals == 4:
                case 5 when betrayals == 5:
                    _outedSpies.Add(missionPlayer.PlayerId);
                    break;
            }
        }
    }

    private void RecordMissionData()
    {
        if (Team == TeamModel.Resistance)
        {
            if (_gameDetails.MissionRounds.TryGetValue(_gameDetails.CurrentMissionRound, out var missionSuccessful)) // true if resistance win
            {
                foreach(var missionPlayer in _gameDetails.MissionTeam!.Where(p => p.PlayerId != PlayerId))
                {
                    var playerVariables = GetPlayerVariables(missionPlayer.PlayerId);
                    if (missionSuccessful)
                    {
                        playerVariables.WentOnSuccessfulMission++;
                    }
                    else
                    {
                        var betrayals = _gameDetails.MissionOutcome.Count(mo => !mo);
                        OutedSpiesCheck(missionPlayer, betrayals);

                        playerVariables.WentOnFailedMission += playerVariables.WentOnFailedMission > 0
                                                                   ? playerVariables.WentOnFailedMission
                                                                   : 1 * _gameDetails.CurrentMissionRound *
                                                                     _gameDetails.MissionSize / betrayals;
                    }
                }

                foreach(var player in _gameDetails.PlayersDetails!.Where(player => player.PlayerId != PlayerId))
                {
                    // suss if player did not want a successful mission to happen
                    var playerVariables = GetPlayerVariables(player.PlayerId);
                    switch(missionSuccessful)
                    {
                        case true when !player.ApprovedMissionTeam:
                            GetPlayerVariables(player.PlayerId).RejectedTeamSuccessfulMission += 2 * _gameDetails.CurrentMissionRound;
                            break;
                        case false when player.ApprovedMissionTeam:
                            playerVariables.VotedForFailedMission += 2 * _gameDetails.CurrentMissionRound;
                            break;
                    }

                    // Proposer
                    if (!player.IsMissionLeader) continue;
                    if (missionSuccessful) continue;
                    //different to source
                    playerVariables.ProposedTeamFailedMission += 3 * playerVariables.ProposedTeamFailedMission * _gameDetails.CurrentMissionRound;

                    // If proposer was in the mission that failed
                    if (_gameDetails.MissionTeam.Any(p => p.PlayerId == player.PlayerId))
                    {
                        playerVariables.ProposedTeamFailedMission += 3 * _gameDetails.CurrentMissionRound;
                    }
                }
            }

            // Run Predictions
            if (_gameDetails.MissionRounds.ContainsValue(false) || _gameDetails.CurrentMissionRound > 2)
            {
                _playerIdToSpyPredictions = NaiveBayesClassifierService.GetPredictions(_playerIdToGameData, _trainingData);
            }
        }
    }

    private void RecordVoteData()
    {
        if (Team == TeamModel.Resistance)
        {
            var missionLeader = GetMissionLeader();
            if (missionLeader!.PlayerId != PlayerId)
            {
                if (_gameDetails.CurrentMissionRound > 1)
                {
                    var missionLeaderPlayerVariables = GetPlayerVariables(missionLeader.PlayerId);

                    // If Proposer proposed a mission that contains no successful mission members
                    var trustingMembersCount = _gameDetails.MissionTeam!.Count(missionMember => GetPlayerVariables(missionMember.PlayerId).WentOnSuccessfulMission > 0);
                    if (trustingMembersCount == 0)
                    {
                        // Proposed a team that havent even been on missions
                        missionLeaderPlayerVariables.ProposedTeamThatHaventBeenOnMissions += 2 * _gameDetails.CurrentMissionRound;
                    }

                    // If Proposer proposed a mission that contains previously failed mission members
                    var untrustingMembersCount = _gameDetails.MissionTeam!.Count(missionMember => GetPlayerVariables(missionLeader.PlayerId).WentOnFailedMission > 0);
                    if (untrustingMembersCount != 0)
                    {
                        missionLeaderPlayerVariables.ProposedTeamHasUnsuccessfulMembers += 4 * _gameDetails.CurrentMissionRound * untrustingMembersCount;
                    }
                }
            }

            foreach(var player in _gameDetails.PlayersDetails!)
            {
                if (player.PlayerId == PlayerId) continue;
                var playerVariables = GetPlayerVariables(player.PlayerId);
                if (!player.ApprovedMissionTeam)
                {
                    playerVariables.VotedAgainstTeamProposal += _gameDetails.CurrentMissionRound;
                    // If they say no to teams that contain successful mission members its suss
                    var successfulMissionsMembers = _gameDetails.MissionTeam!.Count(missionMember => GetPlayerVariables(missionMember.PlayerId).WentOnSuccessfulMission > 0);
                    playerVariables.VotedNoTeamHasSuccessfulMembers += 3 * _gameDetails.CurrentMissionRound * successfulMissionsMembers;
                }

                if (player.ApprovedMissionTeam)
                {
                    // If they vote yes for a mission that contains members that prevousily failed missions its suss
                    var failedPriorMissionsMembers = _gameDetails.MissionTeam!.Count(missionMember => GetPlayerVariables(missionLeader.PlayerId).WentOnFailedMission > 0);
                    playerVariables.VotedForTeamHasUnsuccessfulMembers += 4 * _gameDetails.CurrentMissionRound * failedPriorMissionsMembers > 0 ? failedPriorMissionsMembers : 1;

                    // Voted For A mission they are not on
                    if (_gameDetails.MissionTeam.All(p => p.PlayerId != player.PlayerId))
                    {
                        playerVariables.VotedForMissionNotOn += _gameDetails.CurrentMissionRound;
                    }
                }
            }

            // Run predictions after significant data is collected
            if (_gameDetails.MissionRounds.ContainsValue(false) || _gameDetails.CurrentMissionRound > 2 || _gameDetails.VoteFailedCount > 2)
            {
                _playerIdToSpyPredictions = NaiveBayesClassifierService.GetPredictions(_playerIdToGameData, _trainingData);
            }
        }
    }

    private List<PlayerDetailsModel> ResistanceProposal(int missionSize, List<PlayerDetailsModel> proposedMissionTeam)
    {
        // For the First Mission as Resistance
        // Pick at Random
        if (_gameDetails.CurrentMissionRound == 1)
        {
            proposedMissionTeam.AddRange(GetRandomPlayers(missionSize));
            return proposedMissionTeam;
        }

        var pool = _gameDetails.PlayersDetails!.Where(p => p.PlayerId != PlayerId).ToList();

        // Filter All Outed Spies Out
        pool = pool.Where(p => _outedSpies.All(os => os != p.PlayerId)).ToList();

        // Pick agents that are predicted not to be spies
        if (_playerIdToSpyPredictions != null && _playerIdToSpyPredictions.ContainsValue(true))
        {
            // Filter Predicted Spies
            pool = pool.Where(p => _playerIdToSpyPredictions.Any(sp => sp.Key == p.PlayerId && !sp.Value)).ToList();


            if (pool.Count >= missionSize)
            {
                proposedMissionTeam.AddRange(pool.Take(missionSize));
                return proposedMissionTeam;
            }

            if (pool.Count > 0)
            {
                proposedMissionTeam.AddRange(pool);
                missionSize -= pool.Count;
                if (missionSize == 0)
                {
                    return proposedMissionTeam;
                }
            }
        }

        // If still need more members
        // Pick Members that are that are the most trusting 

        pool = pool.Where(p => _playerIdToGameData.Any(sp => sp.Key == p.PlayerId && sp.Value.WentOnSuccessfulMission >= sp.Value.WentOnFailedMission))
                   .Where(p => proposedMissionTeam.All(pmt => pmt.PlayerId != p.PlayerId))
                   .ToList();
        if (pool.Count >= missionSize)
        {
            proposedMissionTeam.AddRange(pool.Take(missionSize));
            return proposedMissionTeam;
        }

        if (pool.Count > 0)
        {
            proposedMissionTeam.AddRange(pool);
            missionSize -= pool.Count;
        }

        // last resort add random members but as long as they are not outed Spies
        //proposedMissionTeam.AddRange(GetRandomPlayers(missionSize));
        proposedMissionTeam.AddRange(_gameDetails.PlayersDetails
                                                 .Where(p => _outedSpies.All(os => os != p.PlayerId))
                                                 .Where(p => p.PlayerId != PlayerId && proposedMissionTeam.All(pmt => pmt.PlayerId != p.PlayerId))
                                                 .Take(missionSize));


        return proposedMissionTeam;
    }

    private void SetName()
    {
        Name = _randomBotNames.MinBy(x => Guid.NewGuid());
    }

    private List<PlayerDetailsModel> SpyProposal(int missionSize, List<PlayerDetailsModel> proposedMissionTeam)
    {
        // Put Self On team With Only Other Resistance Members
        if (_gameDetails.CurrentMissionRound != 4)
        {
            proposedMissionTeam.AddRange(
                                         GetRandomResistanceTeamPlayers(missionSize)
                                        );
            return proposedMissionTeam;
        }

        var spyWins = _gameDetails.MissionRounds.Values.Count(mo => !mo);
        // Need To Win Mission 4 To Win Game
        if (spyWins == 1)
        {
            // Add 1 More Spy To Team If Need 2 Betrayals
            if (_gameDetails.PlayersDetails!.Count > 6 && _gameDetails.CurrentMissionRound == 4)
            {
                // Add the 1 Spy
                proposedMissionTeam.AddRange(GetRandomSpyTeamPlayers(1));
                missionSize--;
                // Add the Resistance Players
                proposedMissionTeam.AddRange(
                                             GetRandomResistanceTeamPlayers(missionSize)
                                            );
                return proposedMissionTeam;
            }
        }

        // Mission 4 but don't need to win mission 4 OR
        // Need to Win mission 4 But only need 1 Betrayal
        proposedMissionTeam.AddRange(GetRandomResistanceTeamPlayers(missionSize));
        return proposedMissionTeam;
    }

    #endregion

    #region Public Methods

    public void GetChoice()
    {
        Console.WriteLine("asked for choice");
    }

    public bool GetMissionChoice()
    {
        return Team != TeamModel.Spy || GetSpyBetrayal();
    }

    public List<PlayerDetailsModel> GetMissionProposal()
    {
        var missionSize = GetMissionSize(_gameDetails.CurrentMissionRound, _gameDetails.PlayersDetails!.Count);
        var proposedMissionTeam = new List<PlayerDetailsModel>
                                  {
                                      // Always Add Yourself
                                      GetPlayerDetails()
                                  };
        missionSize--;

        return Team == TeamModel.Resistance ? ResistanceProposal(missionSize, proposedMissionTeam) : SpyProposal(missionSize, proposedMissionTeam);
    }

    public string GetName()
    {
        return Name;
    }

    public string GetSpyPredictions()
    {
        var spyPrediction = "";
        if (Team == TeamModel.Resistance)
        {
            if (_playerIdToSpyPredictions.Values.All(isSpy => isSpy == false) && _outedSpies.Count == 0)
            {
                return "I don't know who the spies are yet";
            }

            if (_playerIdToSpyPredictions.Values.Any(isSpy => isSpy))
            {
                spyPrediction += "I Think ";
                foreach(var playerIdToSpyPrediction in _playerIdToSpyPredictions)
                {
                    // Predicted to be a spy
                    if (!playerIdToSpyPrediction.Value) continue;
                    var player = _gameDetails.PlayersDetails!.FirstOrDefault(p => p.PlayerId == playerIdToSpyPrediction.Key);
                    spyPrediction += player!.UserName + " ";
                }

                spyPrediction += "Are Spies. ";
            }


            if (_outedSpies.Count > 0)
            {
                spyPrediction += "I am Certain ";
                foreach(var outedSpyPlayerId in _outedSpies)
                {
                    var player = _gameDetails.PlayersDetails!.FirstOrDefault(p => p.PlayerId == outedSpyPlayerId);

                    spyPrediction += player!.UserName + " ";
                }

                spyPrediction += "Are Spies. ";
            }
        }
        // else if spy choose random resistance players
        else
        {
            spyPrediction += "I Think ";
            var randomPlayers = GetRandomResistanceTeamPlayers(GetHowManySpiesInGame());
            foreach(var randomPlayer in randomPlayers)
            {
                spyPrediction += randomPlayer!.UserName + " ";
            }

            spyPrediction += "Are Spies. ";
        }

        return spyPrediction;
    }

    public TeamModel GetTeam()
    {
        return Team;
    }

    public bool GetVote()
    {
        // Always return true if Agent is the Mission Proposer
        var missionLeader = GetMissionLeader();
        if (missionLeader!.PlayerId == PlayerId)
        {
            return true;
        }

        return Team == TeamModel.Spy ? GetSpyVote(missionLeader) : GetResistanceVote(missionLeader);
    }

    public void SetPlayerId(Guid playerId)
    {
        PlayerId = playerId;
    }


    public void SetTeam(TeamModel team)
    {
        Team = team;
    }

    public void Update(GameDetailsModel gameDetails)
    {
        _gameDetails = gameDetails;
        if (_beginningOfGame)
        {
            //_playerData = new List<PlayerDetailsModel>(_gameDetails.PlayersDetails!);
            _playerIdToGameData = new Dictionary<Guid, PlayerVariablesModel>();
            foreach(var player in _gameDetails.PlayersDetails!)
            {
                _playerIdToGameData.Add(player.PlayerId, new PlayerVariablesModel());
            }

            _beginningOfGame = false;
        }

        switch(_gameDetails.GameStage)
        {
            case GameStageModel.VoteResults:
                RecordVoteData();
                break;
            case GameStageModel.MissionResults:
                RecordMissionData();
                break;
            case GameStageModel.GameOverSpiesWon:
            case GameStageModel.GameOverResistanceWon:
                break;
        }
    }

    #endregion
}
