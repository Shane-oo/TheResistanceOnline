using TheResistanceOnline.GamePlay.PlayerModels.BotModels;

namespace TheResistanceOnline.GamePlay.BotModels;

public interface ISpyBotModel: IBotModel
{
    // will have abstract functions that only spy bots can do and must implement
    // will have functions that all bots can do and must implement

    public void DoASpyBotThing();
}
