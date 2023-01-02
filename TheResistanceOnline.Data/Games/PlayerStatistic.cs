using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;
using TheResistanceOnline.Data.Users;

namespace TheResistanceOnline.Data.Games;

[UsedImplicitly]
[Table("PlayerStatistics")]
public class PlayerStatistic
{
    #region Properties

    public int Id { get; set; }

    [CanBeNull]
    public string UserId { get; set; }

    [CanBeNull]
    public User User { get; set; }

    public int GameId { get; set; }

    public Game Game { get; set; }

    public int Team { get; set; }
    
    public string PlayerId { get; set; }

    #endregion
}
