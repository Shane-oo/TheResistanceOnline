using TheResistanceOnline.GamePlay.BotModels;

namespace TheResistanceOnline.GamePlay.PlayerModels.BotModels;

public interface IResistanceBotModel: IBotModel
{
    // will have functions that only resistance bots can do and must implement
    // will have functions that all bots can do and must implement
    public void DoAResistanceBotThing();
}
