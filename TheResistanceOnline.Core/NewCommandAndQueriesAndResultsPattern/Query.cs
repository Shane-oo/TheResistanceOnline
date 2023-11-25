using MediatR;
using TheResistanceOnline.Data.Entities;

namespace TheResistanceOnline.Core.NewCommandAndQueriesAndResultsPattern;

public interface IQuery<TResponse>: IRequest<Result<TResponse>>
{
    
}

public class Query<TResponse>: ICommand<TResponse>
{
    public UserId UserId { get; set; }

    public Roles UserRole { get; set; }

    protected Query(UserId userId, Roles userRole)
    {
        UserId = userId;
        UserRole = userRole;
    }

}
