using MediatR;
using TheResistanceOnline.Core.Exchange.Responses;

namespace TheResistanceOnline.Core.NewCommandAndQueriesAndResultsPattern;

public interface IQueryHandler<in TQuery, TResponse>: IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}
