using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;

namespace TheResistanceOnline.Data.Games;

[UsedImplicitly]
[Table("GamePlayerValues")]
public class GamePlayerValue
{
    #region Properties

    public int Id { get; set; }

    public int GameId { get; set; }

    public Game Game { get; set; }

    // Json Column
    
    #endregion
}
