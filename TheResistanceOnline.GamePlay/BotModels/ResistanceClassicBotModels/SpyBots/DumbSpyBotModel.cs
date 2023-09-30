using TheResistanceOnline.GamePlay.PlayerModels;

namespace TheResistanceOnline.GamePlay.BotModels.ResistanceClassicBotModels.SpyBots;

public class DumbSpyBotModel: SpyPlayerModel, ISpyBotModel
{
    public DumbSpyBotModel(string name): base(name, true)
    {
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
}
