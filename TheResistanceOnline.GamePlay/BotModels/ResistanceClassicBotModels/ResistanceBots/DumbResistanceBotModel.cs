using TheResistanceOnline.GamePlay.PlayerModels;

namespace TheResistanceOnline.GamePlay.BotModels.ResistanceClassicBotModels.ResistanceBots;

public class DumbResistanceBotModel: ResistancePlayerModel, IResistanceBotModel
{
    #region Construction

    public DumbResistanceBotModel(string name): base(name, true)
    {
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
}
