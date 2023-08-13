using MediatR;

namespace TheResistanceOnline.Core.Commands;

public class CommandBase<TResponse>: IRequest<TResponse> // for commands that return a response
{
    public Guid CommandId { get; set; }

    public Guid UserId { get; set; }

    public CommandBase()
    {
        CommandId = Guid.NewGuid();
    }

    public CommandBase(Guid userId)
    {
        UserId = userId;
    }
}

public class CommandBase: CommandBase<Unit> // for commands that return default (nothing)
{
    public CommandBase()
    {
    }

    public CommandBase(Guid userId): base(userId)
    {
    }
}
