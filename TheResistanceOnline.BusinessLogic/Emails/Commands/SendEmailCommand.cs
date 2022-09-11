using JetBrains.Annotations;
using TheResistanceOnline.BusinessLogic.Core.Commands;

namespace TheResistanceOnline.BusinessLogic.Emails.Commands
{
    public class SendEmailCommand: CommandBase
    {
        #region Properties
        [NotNull]
        public string EmailBody { get; set; }
        [NotNull]
        public string EmailSubject { get; set; }
        [NotNull]
        public string EmailTo { get; set; }

        #endregion
    }
}
