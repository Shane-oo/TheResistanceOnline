using JetBrains.Annotations;
using TheResistanceOnline.BusinessLogic.Games.Models;

namespace TheResistanceOnline.BusinessLogic.Games.BotObservers;

public class BayesBotObserver: IBotObserver
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

    private GameDetailsModel _gameDetails = new();
    private static readonly Random _random = new();

    private readonly List<string> _randomBotNames = new()
                                                    {
                                                        "WALL - E", "R2D2", "K9", "Optimus Prime", "Rosie", "Bender", "C-3PO", "HAL 9000", "Data", "ASIMO", "The Terminator",
                                                        "Micro", "EVA", "RAM", "Sputnik", "Humanoid", "Chip", "Robo", "Robocop", "Alpha", "Beta", "Gamma", "Siri",
                                                        "Raspberry Pie", "AstroBoy", "Chappie", "Ultron", "Omega", "Hydra", "Pixels", "Shane", "Brandon", "Hamish", "Chloe"
                                                    };

    #endregion

    #region Properties

    public Guid PlayerId { get; set; }

    private string Name { get; set; }


    private TeamModel Team { get; set; }

    #endregion

    #region Construction

    public BayesBotObserver()
    {
        SetName();
    }

    #endregion

    #region Private Methods

    [CanBeNull]
    private PlayerDetailsModel GetMissionLeader()
    {
        return _gameDetails.PlayersDetails?.FirstOrDefault(p => p.IsMissionLeader);
    }

    private PlayerDetailsModel GetPlayerDetails()
    {
        return _gameDetails.PlayersDetails?.FirstOrDefault(p => p.PlayerId == PlayerId);
    }

    private List<PlayerDetailsModel> GetRandomPlayers(int amount)
    {
        var desiredList = new List<PlayerDetailsModel>();
        var count = 0;
        while(count != amount)
        {
            var randomPlayer = _gameDetails.PlayersDetails![_random.Next(_gameDetails.PlayersDetails.Count)];

            // check for if randomly got the same person again
            if (desiredList.Any(p => p.PlayerId == randomPlayer.PlayerId)
                || randomPlayer.PlayerId == PlayerId)
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
        //todo
        return true;
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

    private void RecordVoteData()
    {
        if (Team == TeamModel.Resistance)
        {
            var missionLeader = GetMissionLeader();
            if (missionLeader!.PlayerId != PlayerId)
            {
                if (_gameDetails.CurrentMissionRound > 1)
                {
                    // If Proposer proposed a mission that contains no successful mission members
                    var trustingMembersCount = _gameDetails.MissionTeam!.Count(p => p.WentOnSuccessfulMission != 0);
                    if (trustingMembersCount == 0)
                    {
                        // Proposed a team that havent even been on missions
                        missionLeader.ProposedTeamThatHaventBeenOnMissions += 2 * _gameDetails.CurrentMissionRound;
                    }

                    // If Proposer proposed a mission that contains previously failed mission members
                    var untrustingMembersCount = _gameDetails.MissionTeam!.Count(p => p.WentOnFailedMission != 0);
                    if (untrustingMembersCount != 0)
                    {
                        missionLeader.ProposedTeamHasUnsuccessfulMembers += 4 * _gameDetails.CurrentMissionRound * untrustingMembersCount;
                    }
                }
            }

            foreach(var player in _gameDetails.PlayersDetails!)
            {
                if (player.PlayerId == PlayerId) continue;

                if (!player.ApprovedMissionTeam)
                {
                    player.VotedAgainstTeamProposal += _gameDetails.CurrentMissionRound;
                    // If they say no to teams that contain successful mission members its suss
                    var successfulMissionsMembers = _gameDetails.MissionTeam!.Count(p => p.WentOnSuccessfulMission != 0);
                    player.VotedNoTeamHasSuccessfulMembers += 3 * _gameDetails.CurrentMissionRound * successfulMissionsMembers;
                }

                if (player.ApprovedMissionTeam)
                {
                    // If they vote yes for a mission that contains members that prevousily failed missions its suss
                    var failedPriorMissionsMembers = _gameDetails.MissionTeam!.Count(p => p.WentOnFailedMission != 0);
                    player.VotedForTeamHasUnsuccessfulMembers += 4 * _gameDetails.CurrentMissionRound * failedPriorMissionsMembers;

                    // Voted For A mission they are not on
                    if (_gameDetails.MissionTeam.All(p => p.PlayerId != player.PlayerId))
                    {
                        player.VotedForMissionNotOn += _gameDetails.CurrentMissionRound;
                    }
                }
            }

            // Run Predictions todo
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

        //todo after bayes formula added
        proposedMissionTeam.AddRange(GetRandomPlayers(missionSize));
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
        var proposedMissionTeam = new List<PlayerDetailsModel>();
        // Always Add Yourself
        proposedMissionTeam.Add(GetPlayerDetails());
        missionSize--;

        return Team == TeamModel.Resistance ? ResistanceProposal(missionSize, proposedMissionTeam) : SpyProposal(missionSize, proposedMissionTeam);
    }

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

    public string GetName()
    {
        return Name;
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

        switch(_gameDetails.GameStage)
        {
            case GameStageModel.VoteResults:
                RecordVoteData();
                break;
            case GameStageModel.MissionResults:
                break;
        }
    }

    #endregion
}

