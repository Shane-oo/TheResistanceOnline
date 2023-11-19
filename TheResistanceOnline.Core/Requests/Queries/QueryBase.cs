using MediatR;
using TheResistanceOnline.Data.Entities;

namespace TheResistanceOnline.Core.Requests.Queries;

public interface IQuery<out TResponse>: IRequest<TResponse>, IRequestBase
{
}

public class QueryBase<TResponse>: IQuery<TResponse>
{
    #region Properties

    public string ConnectionId { get; set; }

    public UserId UserId { get; set; }

    public Roles UserRole { get; set; }

    #endregion

    #region Construction

    protected QueryBase()
    {
        UserId = new UserId(Guid.Empty);
        UserRole = Roles.None;
        ConnectionId = string.Empty;
    }

    protected QueryBase(UserId userId, Roles userRole)
    {
        UserId = userId;
        UserRole = userRole;
    }

    protected QueryBase(UserId userId, Roles userRole, string connectionId)
    {
        UserId = userId;
        UserRole = userRole;
        ConnectionId = connectionId;
    }

    #endregion
}
