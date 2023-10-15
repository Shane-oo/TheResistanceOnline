using TheResistanceOnline.GamePlay.GameModels;
using TheResistanceOnline.GamePlay.ObserverPattern;
using TheResistanceOnline.GamePlay.PlayerModels;

namespace TheResistanceOnline.GamePlay.BotModels.ResistanceClassicBotModels.SpyBots;

public class DumbSpyBotModel: SpyPlayerModel, ISpyBotModel
{
    private readonly IGameModelSubject _gameModel;

    public DumbSpyBotModel(string name, IGameModelSubject gameModel): base(name, true)
    {
        _gameModel = gameModel;
        _gameModel.RegisterObserver(this);
    }

    public override bool Vote()
    {
        Console.WriteLine("THis is the dum spy bot voting");
        return true;
    }

    public void DoABotThing()
    {
        throw new NotImplementedException();
    }

    public void DoASpyBotThing()
    {
        throw new NotImplementedException();
    }

    public void Update()
    {
        Console.WriteLine("Told to update my values from subject");
    }
}
