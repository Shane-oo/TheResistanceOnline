using Microsoft.EntityFrameworkCore;
using TheResistanceOnline.Data.Entities.GameEntities;

namespace TheResistanceOnline.Data.Queries.Games;

//todo this probs shouldnt be here its not shared
public interface IAllGamePlayerValuesDbQuery: IDbQuery<List<GamePlayerValue>>
{
    IAllGamePlayerValuesDbQuery Include(string[] include);
}

public class AllGamePlayerValuesDbQuery: IAllGamePlayerValuesDbQuery

{
    #region Fields

    private readonly Context _context;
    private string[] _include;

    #endregion

    #region Construction

    public AllGamePlayerValuesDbQuery(Context context)
    {
        _context = context;
    }

    #endregion

    #region Public Methods

    public async Task<List<GamePlayerValue>> ExecuteAsync(CancellationToken cancellationToken)
    {
        var query = _context.Set<GamePlayerValue>().AsQueryable();
        if (_include != null)
        {
            query = _include.Aggregate(query, (current, expression) => current.Include(expression));
        }

        var result = await query.ToListAsync(cancellationToken);
        return result;
    }

    public IAllGamePlayerValuesDbQuery Include(string[] include)
    {
        _include = include;
        return this;
    }

    #endregion
}
