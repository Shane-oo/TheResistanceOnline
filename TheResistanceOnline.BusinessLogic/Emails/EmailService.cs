using JetBrains.Annotations;
using SendGrid;
using SendGrid.Helpers.Mail;
using TheResistanceOnline.BusinessLogic.Emails.Commands;
using TheResistanceOnline.Data.Exceptions;
using TheResistanceOnline.Data.Users;

namespace TheResistanceOnline.BusinessLogic.Emails
{
    public interface IEmailService
    {
        Task SendEmailAsync([NotNull] SendEmailCommand command);
    }

    public class EmailService: IEmailService
    {
        #region Public Methods

        public async Task SendEmailAsync(SendEmailCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            // ToDo store safely                                                                                                                      
            var sendGridClient = new SendGridClient("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");
            var emailFrom = new EmailAddress
                            {
                                Email = "theresistanceboardgameonline@gmail.com",
                                Name = "TheResistanceBoardGameOnline"
                            };
            var emailTo = new EmailAddress
                          {
                              Email = command.EmailTo,
                          };
            try
            {
                var email = MailHelper.CreateSingleEmail(emailFrom, emailTo, command.EmailSubject, string.Empty, command.EmailBody);
                await sendGridClient.SendEmailAsync(email);
            }
            catch(Exception ex)
            {
                // ToDo create a better Exception for this - should this be Domain Exception?
                throw new DomainException(typeof(SendEmailCommand), command.EmailTo, ex.Message);
            }
        }

        #endregion
    }
}
