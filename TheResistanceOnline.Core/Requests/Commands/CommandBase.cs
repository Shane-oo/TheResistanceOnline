using MediatR;
using TheResistanceOnline.Data.Entities.UserEntities;

namespace TheResistanceOnline.Core.Requests.Commands;

public interface ICommand<out TResponse>: IRequest<TResponse>, IRequestBase
{
}

public class CommandBase<TResponse>: ICommand<TResponse>
{
    #region Properties

    public Guid CommandId { get; set; }

    public string ConnectionId { get; set; }

    public Guid UserId { get; set; }

    public Roles UserRole { get; set; }

    #endregion

    #region Construction

    public CommandBase()
    {
        CommandId = Guid.NewGuid();
    }

    public CommandBase(Guid userId, Roles userRole)
    {
        UserId = userId;
        UserRole = userRole;
    }

    public CommandBase(Guid userId, Roles userRole, string connectionId)
    {
        UserId = userId;
        UserRole = userRole;
        ConnectionId = connectionId;
    }

    #endregion
}

public class CommandBase: CommandBase<Unit> // for commands that return default (nothing)
{
    #region Construction

    public CommandBase(Guid userId, Roles userRole): base(userId, userRole)
    {
    }

    public CommandBase(Guid userId, Roles userRole, string connectionId): base(userId, userRole, connectionId)
    {
    }

    public CommandBase()
    {
    }

    #endregion
}
