namespace TheResistanceOnline.BusinessLogic.Games.Models;

public interface ISpectatorBotObserver
{
    Dictionary<Guid, PlayerVariablesModel> GetPlayerVariablesDictionary();
}
