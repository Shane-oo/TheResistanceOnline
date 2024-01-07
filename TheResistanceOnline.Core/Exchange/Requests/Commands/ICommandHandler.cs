using MediatR;
using TheResistanceOnline.Core.Exchange.Responses;

namespace TheResistanceOnline.Core.Exchange.Requests;

public interface ICommandHandler<in TCommand>: IRequestHandler<TCommand, Result>
    where TCommand : ICommand
{
}

public interface ICommandHandler<in TCommand, TResponse>: IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : ICommand<TResponse>
{
}
