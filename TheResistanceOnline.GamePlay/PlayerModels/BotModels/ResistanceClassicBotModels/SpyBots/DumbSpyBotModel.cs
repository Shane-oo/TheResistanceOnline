using TheResistanceOnline.GamePlay.BotModels;
using TheResistanceOnline.GamePlay.ObserverPattern;

namespace TheResistanceOnline.GamePlay.PlayerModels.BotModels.ResistanceClassicBotModels.SpyBots;

public class DumbSpyBotModel: SpyPlayerModel, ISpyBotModel
{
    #region Construction

    public DumbSpyBotModel(string name, IGameModelSubject gameModel): base(name, gameModel, true)
    {
    }

    #endregion

    #region Public Methods

    public void DoABotThing()
    {
        throw new NotImplementedException();
    }

    public void VoteForMissionTeam()
    {
        Vote(true);
    }

    public void SelectAMissionTeam()
    {
        var selectedPlayerNames = Players.Take(MissionSize);
        foreach(var selectedPlayerName in selectedPlayerNames)
        {
            PickMissionTeamMember(selectedPlayerName);
        }
    }

    public void DoASpyBotThing()
    {
        throw new NotImplementedException();
    }

    #endregion
}
