using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;
using TheResistanceOnline.Data.Entities;

namespace TheResistanceOnline.Data.Games;

[UsedImplicitly]
[Table("Games")]
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
