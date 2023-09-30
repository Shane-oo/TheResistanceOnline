namespace TheResistanceOnline.GamePlay.BotModels;

// this will also be an observer to GameModel Subject
public interface IBotModel
{
    // will have abstract functions that all bots must implement
    // will have functions that are concrete and all bots do and need

    public void DoABotThing();
}
