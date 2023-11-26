using TheResistanceOnline.GamePlay.Common;
using TheResistanceOnline.GamePlay.ObserverPattern;

namespace TheResistanceOnline.GamePlay.PlayerModels;

public abstract class PlayerModel: IObserver
{
    #region Fields

    private readonly IGameModelSubject _gameModel;

    #endregion

    #region Properties

    public bool IsBot { get; set; }

    public bool IsMissionLeader { get; set; }

    public string MissionLeader { get; set; }

    public string Name { get; set; }

    public Team Team { get; set; }

    protected int MissionSize { get; set; }

    protected List<string> MissionTeamMembers { get; set; }

    protected List<string> Players { get; set; }

    #endregion

    #region Construction

    protected PlayerModel(string name, IGameModelSubject gameModel)
    {
        Name = name;
        _gameModel = gameModel;
        _gameModel.RegisterObserver(this);
    }

    protected PlayerModel(string name, IGameModelSubject gameModel, bool isBot)
    {
        Name = name;
        _gameModel = gameModel;
        IsBot = isBot;
        _gameModel.RegisterObserver(this);
    }

    #endregion

    #region Public Methods

    public virtual void PickMissionTeamMember(string selectedPlayerName = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(selectedPlayerName);

        _gameModel.AddMissionTeamMember(selectedPlayerName);
    }

    public virtual void RemoveMissionTeamMember(string selectedPlayerName = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(selectedPlayerName);

        _gameModel.RemoveMissionTeamMember(selectedPlayerName);
    }

    public void Update()
    {
        MissionTeamMembers = _gameModel.MissionTeam;
        Players = _gameModel.PlayerNames;
        MissionSize = _gameModel.MissionSize;
        MissionLeader = _gameModel.MissionLeader;
    }

    public abstract bool Vote();

    #endregion
}
