using TheResistanceOnline.GamePlay.Common;
using TheResistanceOnline.GamePlay.ObserverPattern;
using TheResistanceOnline.GamePlay.PlayerModels.BotModels;

namespace TheResistanceOnline.GamePlay.PlayerModels;

public abstract class PlayerModel: IObserver
{
    #region Fields

    private readonly IGameModelSubject _gameModel;

    #endregion

    #region Properties

    public IBotModel BotModel { get; set; }

    public bool IsBot { get; set; }

    public bool IsMissionLeader { get; set; }

    public bool? MissionChoice { get; set; }

    public string MissionLeader { get; set; }

    public string Name { get; set; }

    public Team Team { get; set; }

    public bool? VoteChoice { get; set; }

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

    public void PickMissionTeamMember(string selectedPlayerName)
    {
        _gameModel.AddMissionTeamMember(selectedPlayerName);
    }

    public void RemoveMissionTeamMember(string selectedPlayerName)
    {
        _gameModel.RemoveMissionTeamMember(selectedPlayerName);
    }

    public virtual void SubmitMissionOutcome(bool decision)
    {
        MissionChoice = decision;
    }

    public void Update()
    {
        MissionTeamMembers = _gameModel.MissionTeam.Select(p => p.Name).ToList();
        Players = _gameModel.PlayerNames;
        MissionSize = _gameModel.MissionSize;
        MissionLeader = _gameModel.MissionLeader.Name;
    }


    public void Vote(bool decision)
    {
        VoteChoice = decision;
    }

    #endregion
}
