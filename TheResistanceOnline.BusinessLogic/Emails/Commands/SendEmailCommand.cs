using TheResistanceOnline.BusinessLogic.Core.Commands;

namespace TheResistanceOnline.BusinessLogic.Emails.Commands
{
    public class SendEmailCommand: CommandBase
    {
        #region Properties

        public string? EmailBody { get; set; }

        public string? EmailSubject { get; set; }

        public string? EmailTo { get; set; }

        #endregion
    }
}
