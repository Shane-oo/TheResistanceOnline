namespace TheResistanceOnline.BusinessLogic.Core.Commands
{
    public interface ICommand: ICommandBase
    {
        Guid CommandId { get; set; }

        Guid CorrelationId { get; set; }

        Guid ParentCommandId { get; set; }
    }
}
