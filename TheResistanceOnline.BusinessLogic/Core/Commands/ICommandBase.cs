namespace TheResistanceOnline.BusinessLogic.Core.Commands
{
    public interface ICommandBase
    {
        CancellationToken CancellationToken { get; set; }
    }
}
