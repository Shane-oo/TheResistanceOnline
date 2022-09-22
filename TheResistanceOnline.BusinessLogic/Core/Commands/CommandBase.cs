using AutoMapper.Configuration.Annotations;

namespace TheResistanceOnline.BusinessLogic.Core.Commands
{
    public class CommandBase: ICommand
    {
        #region Properties

        public CancellationToken CancellationToken { get; set; }

        [Ignore]
        public Guid CommandId { get; set; }

        [Ignore]
        public Guid CorrelationId { get; set; }

        [Ignore]
        public Guid ParentCommandId { get; set; }

        [Ignore]
        public string UserId { get; set; }

        #endregion

        #region Construction

        public CommandBase()
        {
            CommandId = Guid.NewGuid();
            CorrelationId = Guid.NewGuid();
        }

        public CommandBase(string userId)
        {
            CommandId = Guid.NewGuid();
            UserId = userId;
        }

        public CommandBase(ICommand parentCommand)
        {
            ParentCommandId = parentCommand.CommandId;
            CorrelationId = parentCommand.CorrelationId;
            CancellationToken = parentCommand.CancellationToken;
        }

        #endregion
    }
}
