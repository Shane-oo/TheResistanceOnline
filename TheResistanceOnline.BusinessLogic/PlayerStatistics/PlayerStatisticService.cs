using TheResistanceOnline.BusinessLogic.Games.Commands;
using TheResistanceOnline.Data.PlayerStatistics;

namespace TheResistanceOnline.BusinessLogic.PlayerStatistics;

public interface IPlayerStatisticService
{
    List<PlayerStatistic> CreatePlayerStatistics(SaveGameCommand command, int winningTeam);
}

public class PlayerStatisticService: IPlayerStatisticService
{
    #region Public Methods

    public List<PlayerStatistic> CreatePlayerStatistics(SaveGameCommand command, int winningTeam)
    {
        return command.GameDetails.PlayersDetails!.Where(p => !p.IsBot)
                      .Select(playerDetails => new PlayerStatistic
                                               {
                                                   UserId = playerDetails.PlayerId, // Real Users Player Id is Their User Id
                                                   Team = (int)playerDetails.Team,
                                                   Won = (int)playerDetails.Team == winningTeam
                                               })
                      .ToList();
    }

    #endregion
}
