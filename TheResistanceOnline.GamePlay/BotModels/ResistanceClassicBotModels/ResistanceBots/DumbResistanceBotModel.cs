using TheResistanceOnline.GamePlay.GameModels;
using TheResistanceOnline.GamePlay.ObserverPattern;
using TheResistanceOnline.GamePlay.PlayerModels;

namespace TheResistanceOnline.GamePlay.BotModels.ResistanceClassicBotModels.ResistanceBots;

public class DumbResistanceBotModel: ResistancePlayerModel, IResistanceBotModel
{
    #region Construction
    private readonly IGameModelSubject _gameModel;

    public DumbResistanceBotModel(string name, IGameModelSubject gameModel): base(name, true)
    {
        _gameModel = gameModel;
        _gameModel.RegisterObserver(this);
    }

    #endregion

    #region Public Methods

    public void DoABotThing()
    {
        Console.WriteLine("Bot thing");
    }

    public void DoAResistanceBotThing()
    {
        throw new NotImplementedException();
    }

    public override bool Vote()
    {
        Console.WriteLine("THis is the dumb resistance bot voting");
        DoABotThing();
        return true;
    }

    #endregion

    public void Update()
    {
        Console.WriteLine("Told to update my values from subject");
    }
}
