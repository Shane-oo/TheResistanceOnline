using MediatR;

namespace TheResistanceOnline.Core.NewCommandAndQueriesAndResultsPattern;

public interface IQueryHandler<in TQuery, TResponse>: IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}
