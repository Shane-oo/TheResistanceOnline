using TheResistanceOnline.Data.Core.Queries;
using TheResistanceOnline.Data.Games;

namespace TheResistanceOnline.BusinessLogic.Games.DbQueries;

public interface IAllGamePlayerValuesDbQuery: IDbQuery<List<GamePlayerValue>>
{
    IAllGamePlayerValuesDbQuery Include(string[] include);

}
