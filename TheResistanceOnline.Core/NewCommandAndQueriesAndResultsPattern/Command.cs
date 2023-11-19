using MediatR;
using TheResistanceOnline.Data.Entities;

namespace TheResistanceOnline.Core.NewCommandAndQueries;

public interface ICommand: IRequest<Result>, IBaseCommand
{
}

public interface ICommand<TResponse>: IRequest<Result<TResponse>>, IBaseCommand
{
}

public interface IBaseCommand
{
}

public class Command<TResponse>: ICommand<TResponse>
{
    #region Properties

    public UserId UserId { get; set; }

    public Roles UserRole { get; set; }

    #endregion

    #region Construction

    public Command(UserId userId, Roles userRole)
    {
        UserId = userId;
        UserRole = userRole;
    }

    #endregion
}

public class Command: Command<Unit>
{
    #region Construction

    public Command(UserId userId, Roles userRole): base(userId, userRole)
    {
    }

    #endregion
}
