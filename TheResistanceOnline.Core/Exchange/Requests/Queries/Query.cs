using MediatR;
using TheResistanceOnline.Core.Exchange.Responses;
using TheResistanceOnline.Data.Entities;

namespace TheResistanceOnline.Core.NewCommandAndQueriesAndResultsPattern;

public interface IQuery<TResponse>: IRequest<Result<TResponse>>
{
}

public class Query<TResponse>: IQuery<TResponse>
{
    #region Properties

    public UserId UserId { get; set; }

    public Roles UserRole { get; set; }

    #endregion

    #region Construction

    public Query(UserId userId, Roles userRole)
    {
        UserId = userId;
        UserRole = userRole;
    }

    public Query()
    {
    }

    #endregion
}
