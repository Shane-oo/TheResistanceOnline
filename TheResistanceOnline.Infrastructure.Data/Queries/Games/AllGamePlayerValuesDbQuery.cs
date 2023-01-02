using Microsoft.EntityFrameworkCore;
using TheResistanceOnline.BusinessLogic.Games.DbQueries;
using TheResistanceOnline.Data.Games;

namespace TheResistanceOnline.Infrastructure.Data.Queries.Games;

public class AllGamePlayerValuesDbQuery: IAllGamePlayerValuesDbQuery

{
    private readonly Context _context;
    private string[]? _include;

    public AllGamePlayerValuesDbQuery(Context context)
    {
        _context = context;
    }

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
}
