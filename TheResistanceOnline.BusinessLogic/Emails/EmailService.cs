using JetBrains.Annotations;
using SendGrid;
using SendGrid.Helpers.Mail;
using TheResistanceOnline.BusinessLogic.Emails.Commands;
using TheResistanceOnline.BusinessLogic.Settings;
using TheResistanceOnline.Data.Exceptions;

namespace TheResistanceOnline.BusinessLogic.Emails
{
    public interface IEmailService
    {
        void SendEmailAsync([NotNull] SendEmailCommand command);
    }

    public class EmailService: IEmailService
    {
        #region Fields

        private readonly ISettingsService _settings;

        #endregion

        #region Construction

        public EmailService(ISettingsService settings)
        {
            _settings = settings;
        }

        #endregion

        #region Public Methods

        public async void SendEmailAsync(SendEmailCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var sendGridClient = new SendGridClient(_settings.GetAppSettings().SendGridClientToken);
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
