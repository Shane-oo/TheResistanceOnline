using MediatR;
using TheResistanceOnline.Data.Entities.UserEntities;

namespace TheResistanceOnline.Core.Requests.Queries;

public interface IQuery<out TResponse>: IRequest<TResponse>, IRequestBase
{
}

public class QueryBase<TResponse>: IQuery<TResponse>
{
    #region Properties

    public Guid UserId { get; set; }

    public Roles UserRole { get; set; }

    #endregion

    #region Construction

    protected QueryBase()
    {
        UserId = Guid.Empty;
        UserRole = Roles.None;
    }

    protected QueryBase(Guid userId, Roles userRole)
    {
        UserId = userId;
        UserRole = userRole;
    }

    #endregion
}
