using MediatR;
using TheResistanceOnline.Data.Entities;

namespace TheResistanceOnline.Core.NewCommandAndQueriesAndResultsPattern;

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

    protected Command(UserId userId, Roles userRole)
    {
        UserId = userId;
        UserRole = userRole;
    }

    protected Command(UserId userId)
    {
        UserId = userId;
    }

    protected Command()
    {
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
