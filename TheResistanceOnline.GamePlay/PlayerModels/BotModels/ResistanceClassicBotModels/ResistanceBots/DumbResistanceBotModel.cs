using TheResistanceOnline.GamePlay.ObserverPattern;

namespace TheResistanceOnline.GamePlay.PlayerModels.BotModels.ResistanceClassicBotModels.ResistanceBots;

public class DumbResistanceBotModel: ResistancePlayerModel, IResistanceBotModel
{
    #region Construction

    public DumbResistanceBotModel(string name, IGameModelSubject gameModel): base(name, gameModel, true)
    {
    }

    #endregion

    #region Public Methods

    public void DoABotThing()
    {
        Console.WriteLine("Bot thing");
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

    public void DecideMissionOutcome()
    {
        SubmitMissionOutcome(true);
    }


    public void DoAResistanceBotThing()
    {
        throw new NotImplementedException();
    }

    #endregion
}
