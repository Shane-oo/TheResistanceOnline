using AutoMapper;
using JetBrains.Annotations;
using Microsoft.AspNetCore.WebUtilities;
using TheResistanceOnline.BusinessLogic.Core.Queries;
using TheResistanceOnline.BusinessLogic.Emails;
using TheResistanceOnline.BusinessLogic.Emails.Commands;
using TheResistanceOnline.BusinessLogic.Users.Commands;
using TheResistanceOnline.BusinessLogic.Users.DbQueries;
using TheResistanceOnline.BusinessLogic.Users.Models;
using TheResistanceOnline.Data;
using TheResistanceOnline.Data.Exceptions;
using TheResistanceOnline.Data.Users;

namespace TheResistanceOnline.BusinessLogic.Users
{
    public interface IUserService
    {
        Task ConfirmUserEmailAsync([NotNull] UserConfirmEmailCommand command);

        Task CreateUserAsync([NotNull] UserRegisterCommand command);

        Task<UserDetailsModel> GetUserAsync([NotNull] ByIdQuery query);

        Task<User> GetUserByEmailOrNameAsync([NotNull] ByIdAndNameQuery query);

        Task<UserLoginResponse> LoginUserAsync([NotNull] UserLoginCommand command);

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

        private readonly IDataContext _context;

        private readonly IEmailService _emailService;

        private readonly IUserIdentityManager _identityManager;

        private readonly IMapper _mapper;

        #endregion

        #region Construction

        public UserService(IUserIdentityManager identityManager, IEmailService emailService, IDataContext context, IMapper mapper)
        {
            _identityManager = identityManager;
            _emailService = emailService;
            _context = context;
            _mapper = mapper;
        }

        #endregion

        #region Public Methods

        public async Task ConfirmUserEmailAsync(UserConfirmEmailCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var user = new User
                       {
                           Email = command.Email,
                       };
            await _identityManager.ConfirmUsersEmailAsync(user, command.Token);
        }

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


            var token = await _identityManager.CreateIdentityAsync(user, command.Password);
            //todo this is broken
            //await _identityManager.CreateUserRoleAsync(user, "User");

            var param = new Dictionary<string, string>
                        {
                            { "token", token },
                            { "email", user.Email }
                        };
            var callback = QueryHelpers.AddQueryString(command.ClientUri, param);

            var sendEmailCommand = new SendEmailCommand
                                   {
                                       EmailTo = user.Email!,
                                       EmailSubject = "The Resistance Board Game Online - Confirm Email",
                                       CancellationToken = command.CancellationToken,
                                       EmailBody = "<h1> Click To Confirm Email: " + callback
                                   };
            await _emailService.SendEmailAsync(sendEmailCommand);
        }

        public async Task<UserDetailsModel> GetUserAsync(ByIdQuery query)
        {
            if (query == null || string.IsNullOrEmpty(query.UserId))
            {
                throw new ArgumentNullException(nameof(query));
            }

            return _mapper.Map<UserDetailsModel>(await _context.Query<IUserDbQuery>().WithParams(query.UserId)
                                                               .ExecuteAsync(query.CancellationToken));
        }

        public async Task<User> GetUserByEmailOrNameAsync(ByIdAndNameQuery query)
        {
            if (query == null || string.IsNullOrEmpty(query.Name))
            {
                throw new ArgumentNullException(nameof(query));
            }

            return await _context.Query<IUserByNameOrEmailDbQuery>().WithParams(query.Name).ExecuteAsync(query.CancellationToken);
        }


        public async Task<UserLoginResponse> LoginUserAsync(UserLoginCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var user = new User
                       {
                           Email = command.Email,
                       };

            try
            {
                return await _identityManager.LoginUserByEmailAsync(user, command.Password);
            }
            catch(UnauthorizedAccessException)
            {
                await SendResetPasswordAsync(new UserForgotPasswordCommand
                                             {
                                                 CancellationToken = command.CancellationToken,
                                                 ClientUri = command.ClientUri,
                                                 CommandId = command.CommandId,
                                                 CorrelationId = command.CorrelationId,
                                                 Email = command.Email
                                             });

                throw new DomainException(typeof(User), user.Email,
                                          "account is Now locked. instructions have been sent to your email address with a link to reset your password");
            }
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
            var param = new Dictionary<string, string>
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
