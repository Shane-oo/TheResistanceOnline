using TheResistanceOnline.Data.PlayerStatistics;

namespace TheResistanceOnline.Data.Entities.GameEntities;

public class Game: IAuditableEntity
{
    #region Properties

    public int Id { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public int WinningTeam { get; set; }

    public List<PlayerStatistic> PlayerStatistics { get; set; }

    public GamePlayerValue GamePlayerValue { get; set; }

    #endregion
}
