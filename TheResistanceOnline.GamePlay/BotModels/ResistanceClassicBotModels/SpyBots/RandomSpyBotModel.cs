using TheResistanceOnline.GamePlay.GameModels;
using TheResistanceOnline.GamePlay.ObserverPattern;
using TheResistanceOnline.GamePlay.PlayerModels;

namespace TheResistanceOnline.GamePlay.BotModels.ResistanceClassicBotModels.SpyBots;

public class RandomSpyBotModel: SpyPlayerModel, ISpyBotModel
{
    #region Construction

    private readonly IGameModelSubject _gameModel;

    public RandomSpyBotModel(string name, IGameModelSubject gameModel): base(name, true)
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

    public void DoASpyBotThing()
    {
        throw new NotImplementedException();
    }

    public void Update()
    {
        Console.WriteLine("Told to update my values from subject");
    }

    #endregion
}
