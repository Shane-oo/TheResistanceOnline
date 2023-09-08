using MediatR;
using TheResistanceOnline.Data.Entities.UserEntities;

namespace TheResistanceOnline.Core.Requests.Queries;

public interface IQuery<out TResponse>: IRequest<TResponse>, IRequestBase
{
}

public class QueryBase<TResponse>: IQuery<TResponse>
{
    #region Properties

    public string ConnectionId { get; set; }

    public Guid UserId { get; set; }

    public Roles UserRole { get; set; }

    #endregion

    #region Construction

    protected QueryBase()
    {
        UserId = Guid.Empty;
        UserRole = Roles.None;
        ConnectionId = string.Empty;
    }

    protected QueryBase(Guid userId, Roles userRole)
    {
        UserId = userId;
        UserRole = userRole;
    }

    protected QueryBase(Guid userId, Roles userRole, string connectionId)
    {
        UserId = userId;
        UserRole = userRole;
        ConnectionId = connectionId;
    }

    #endregion
}
