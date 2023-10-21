using TheResistanceOnline.Core.Exceptions;
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

    public int Mission { get; private set; } = 1;

    public string MissionLeader => Players.First(c => c.Value.IsMissionLeader).Key;

    public int MissionSize => GetMissionSize();

    public List<PlayerModel> MissionTeam { get; private set; }

    public Phase Phase { get; private set; } = Phase.MissionBuild;

    public List<string> PlayerNames => Players.Keys.ToList();

    public Dictionary<string, PlayerModel> Players { get; } = new();

    public int TotalPlayers => Players.Count;

    public int VoteTrack { get; set; } = 1;

    #endregion

    #region Private Methods

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

    protected void SubmitMissionTeam()
    {
        if (!MissionTeamFull())
        {
        }
    }


    protected void UpdateMission(int mission)
    {
        Mission = mission;
        NotifyObservers();
    }

    protected void UpdateMissionLeader(string missionLeaderName)
    {
        foreach(var playerModel in Players.Values)
        {
            playerModel.IsMissionLeader = missionLeaderName == playerModel.Name;
        }

        NotifyObservers();
    }

    protected void UpdatePhase(Phase phase)
    {
        Phase = phase;
        NotifyObservers();
    }

    protected void UpdateVoteTrack(int voteTrack)
    {
        VoteTrack = voteTrack;
        NotifyObservers();
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

    #endregion

    #region Public Methods

    public void AddMissionTeamMember(PlayerModel player)
    {
        ArgumentNullException.ThrowIfNull(player);

        if (MissionTeamFull())
        {
            return;
        }

        MissionTeam.Add(player);
    }

    public PlayerModel GetPlayerModel(string name)
    {
        var player = Players.FirstOrDefault(p => p.Key == name).Value;
        NotFoundException.ThrowIfNull(player);

        return player;
    }

    public bool MissionTeamFull()
    {
        return MissionTeam.Count == GetMissionSize();
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

    public void RemoveMissionTeamMember(PlayerModel player)
    {
        MissionTeam.Remove(player);
    }

    public void RemoveObserver(IObserver observer)
    {
        _observers.Remove(observer);
    }

    public abstract void SetupGame(List<string> playerUserNames, int botCount);

    #endregion
}
