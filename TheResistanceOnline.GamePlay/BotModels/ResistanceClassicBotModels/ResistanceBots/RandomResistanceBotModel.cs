using TheResistanceOnline.GamePlay.ObserverPattern;
using TheResistanceOnline.GamePlay.PlayerModels;

namespace TheResistanceOnline.GamePlay.BotModels.ResistanceClassicBotModels.ResistanceBots;

public class RandomResistanceBotModel: ResistancePlayerModel, IResistanceBotModel
{
    #region Fields

    private readonly IGameModelSubject _gameModel;
    private static readonly Random _random = new();

    #endregion

    #region Properties

    private int MissionSize { get; set; }

    private List<string> Players { get; set; }

    private string MissionLeader { get; set; }

    #endregion

    #region Construction

    public RandomResistanceBotModel(string name, IGameModelSubject gameModel): base(name, true)
    {
        _gameModel = gameModel;
        _gameModel.RegisterObserver(this);
    }

    #endregion

    #region Public Methods

    public void DoABotThing()
    {
        throw new NotImplementedException();
    }

    public void DoAResistanceBotThing()
    {
        throw new NotImplementedException();
    }

    public override List<string> PickTeam(List<string> chosenPlayers = null)
    {
        // Choose random players for mission
        chosenPlayers = Players
                        .Select(player => new { player, i = _random.Next() })
                        .OrderBy(x => x.i).Take(MissionSize)
                        .Select(x => x.player).ToList();

        return base.PickTeam(chosenPlayers);
    }

    public void Update()
    {
        Players = _gameModel.PlayerNames;
        MissionSize = _gameModel.MissionSize;
        MissionLeader = _gameModel.MissionLeader;
    }

    #endregion
}
