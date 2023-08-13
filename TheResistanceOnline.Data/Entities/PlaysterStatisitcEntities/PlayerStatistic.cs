using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;
using TheResistanceOnline.Data.Entities.GameEntities;
using TheResistanceOnline.Data.Entities.UserEntities;

namespace TheResistanceOnline.Data.PlayerStatistics;

[UsedImplicitly]
[Table("PlayerStatistics")]
public class PlayerStatistic
{
    #region Properties

    public int Id { get; set; }

    public Guid UserId { get; set; }

    public User User { get; set; }

    public int GameId { get; set; }

    public Game Game { get; set; }

    public int Team { get; set; }

    public bool Won { get; set; }

    #endregion
}
