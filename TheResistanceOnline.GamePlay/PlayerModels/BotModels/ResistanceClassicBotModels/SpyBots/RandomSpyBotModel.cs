using TheResistanceOnline.GamePlay.BotModels;
using TheResistanceOnline.GamePlay.ObserverPattern;

namespace TheResistanceOnline.GamePlay.PlayerModels.BotModels.ResistanceClassicBotModels.SpyBots;

public class RandomSpyBotModel: SpyPlayerModel, ISpyBotModel
{
    #region Fields

    private static readonly Random _random = new();

    #endregion

    #region Construction

    public RandomSpyBotModel(string name, IGameModelSubject gameModel): base(name, gameModel, true)
    {
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


    public void SelectAMissionTeam()
    {
        var selectedPlayerNames = Players
                                  .Select(player => new { player, i = _random.Next() })
                                  .OrderBy(x => x.i)
                                  .Take(MissionSize)
                                  .Select(x => x.player);

        foreach(var selectedPlayerName in selectedPlayerNames)
        {
            PickMissionTeamMember(selectedPlayerName);
        }
    }

    public void VoteForMissionTeam()
    {
        // todo Random Vote
        Vote(true);
    }

    #endregion
}
