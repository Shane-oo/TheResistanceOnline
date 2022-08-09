using JetBrains.Annotations;
using Microsoft.AspNetCore.WebUtilities;
using TheResistanceOnline.BusinessLogic.Emails;
using TheResistanceOnline.BusinessLogic.Emails.Commands;
using TheResistanceOnline.BusinessLogic.Users.Commands;
using TheResistanceOnline.Data.Users;

namespace TheResistanceOnline.BusinessLogic.Users
{
    public interface IUserService
    {
        Task CreateUserAsync([NotNull] UserRegisterCommand command);


        Task<string> LoginUserAsync([NotNull] UserLoginCommand command);

        Task ResetUserPasswordAsync([NotNull] UserResetPasswordCommand command);

        Task SendResetPasswordAsync([NotNull] UserForgotPasswordCommand command);
    }

    /*
     * Service for managing both User Functions for login and registration
     * and for managing jwt tokens
     */
    public class UserService: IUserService
    {
        #region Fields

        private readonly IEmailService _emailService;

        private readonly IUserIdentityManager _identityManager;

        #endregion

        #region Construction

        public UserService(IUserIdentityManager identityManager, IEmailService emailService)
        {
            _identityManager = identityManager;
            _emailService = emailService;
        }

        #endregion

        #region Public Methods

        public async Task CreateUserAsync(UserRegisterCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }


            var user = new User
                       {
                           UserName = command.UserName,
                           Email = command.Email
                       };

            await _identityManager.CreateIdentityAsync(user, command.Password);
            await _identityManager.CreateUserRoleAsync(user, "User");
        }


        public async Task<string> LoginUserAsync(UserLoginCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var user = new User
                       {
                           Email = command.Email,
                       };

            return await _identityManager.LoginUserByEmailAsync(user, command.Password);
        }

        public async Task ResetUserPasswordAsync(UserResetPasswordCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var user = new User
                       {
                           Email = command.Email,
                       };

            await _identityManager.ResetPasswordAsync(user, command.Token, command.Password);
        }

        public async Task SendResetPasswordAsync(UserForgotPasswordCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var user = new User
                       {
                           Email = command.Email,
                       };

            var token = await _identityManager.GetPasswordResetTokenAsync(user);
            var param = new Dictionary<string, string?>
                        {
                            { "token", token },
                            { "email", command.Email }
                        };
            var callback = QueryHelpers.AddQueryString(command.ClientUri, param);
            var sendEmailCommand = new SendEmailCommand
                                   {
                                       EmailTo = user.Email!,
                                       EmailSubject = "The Resistance Board Game Online - Reset Password",
                                       CancellationToken = command.CancellationToken,
                                       EmailBody = "<h1> Click To Reset Password: " + callback
                                   };

            await _emailService.SendEmailAsync(sendEmailCommand);
        }

        #endregion
    }
}
