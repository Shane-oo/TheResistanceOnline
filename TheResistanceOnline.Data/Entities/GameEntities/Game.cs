using TheResistanceOnline.Data.PlayerStatistics;

namespace TheResistanceOnline.Data.Entities.GameEntities;

public class Game: IAuditableEntity
{
    #region Properties

    public int Id { get; set; }

    public DateTimeOffset CreatedOn { get; set; }

    public DateTimeOffset? ModifiedOn { get; set; }

    public int WinningTeam { get; set; }

    public List<PlayerStatistic> PlayerStatistics { get; set; }

    public GamePlayerValue GamePlayerValue { get; set; }

    #endregion
}
