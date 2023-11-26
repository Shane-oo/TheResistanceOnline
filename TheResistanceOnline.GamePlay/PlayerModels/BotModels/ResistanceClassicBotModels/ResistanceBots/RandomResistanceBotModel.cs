using TheResistanceOnline.GamePlay.ObserverPattern;

namespace TheResistanceOnline.GamePlay.PlayerModels.BotModels.ResistanceClassicBotModels.ResistanceBots;

public class RandomResistanceBotModel: ResistancePlayerModel, IResistanceBotModel
{
    #region Fields

    private static readonly Random _random = new();

    #endregion

    #region Construction

    public RandomResistanceBotModel(string name, IGameModelSubject gameModel): base(name, gameModel, true)
    {
    }

    #endregion

    #region Public Methods

    public void DoABotThing()
    {
        throw new NotImplementedException();
    }

    public void DoAResistanceBotThing()
    {
        throw new NotImplementedException();
    }

    // Override the PickMissionTeamMember and select random mission member
    public override void PickMissionTeamMember(string selectedPlayerName = null)
    {
        selectedPlayerName = Players
                             .Where(p => !MissionTeamMembers.Contains(p))
                             .Select(player => new { player, i = _random.Next() })
                             .OrderBy(x => x.i).Take(MissionSize)
                             .Select(x => x.player).First();

        base.PickMissionTeamMember(selectedPlayerName);
    }

    #endregion
}