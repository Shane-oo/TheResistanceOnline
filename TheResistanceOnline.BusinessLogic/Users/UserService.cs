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
using TheResistanceOnline.Data.UserSettings;

namespace TheResistanceOnline.BusinessLogic.Users
{
    public interface IUserService
    {
        Task ConfirmUserEmailAsync([NotNull] UserConfirmEmailCommand command);

        Task CreateUserAsync([NotNull] UserRegisterCommand command);

        Task<UserDetailsModel> GetUserByUserIdAsync([NotNull] ByIdQuery query);

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

        private readonly TimeSpan _cooldownMinutes = new(0, 5, 0);

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

        #region Private Methods

        private static void CheckUserExists([CanBeNull] User user)
        {
            if (user == null)
            {
                throw new DomainException(typeof(User), "User Does Not Exist");
            }
        }

        #endregion

        #region Public Methods

        public async Task ConfirmUserEmailAsync(UserConfirmEmailCommand command)
        {
            ArgumentNullException.ThrowIfNull(command);

            var user = await _context.Query<IUserByNameOrEmailDbQuery>()
                                     .WithParams(command.Email)
                                     .ExecuteAsync(command.CancellationToken);
            CheckUserExists(user);
            await _identityManager.ConfirmUsersEmailAsync(user, command.Token);
        }

        public async Task CreateUserAsync(UserRegisterCommand command)
        {
            ArgumentNullException.ThrowIfNull(command);

            var user = new User
                       {
                           UserName = command.UserName,
                           Email = command.Email
                       };

            var token = await _identityManager.CreateIdentityAsync(user, command.Password);
            var param = new Dictionary<string, string>
                        {
                            { "token", token },
                            { "email", user.Email }
                        };
            var callback = QueryHelpers.AddQueryString(command.ClientUri, param);
            var sendEmailCommand = new SendEmailCommand
                                   {
                                       EmailTo = user.Email,
                                       EmailSubject = "The Resistance Board Game Online - Confirm Email",
                                       CancellationToken = command.CancellationToken,
                                       EmailBody = "<h1> Click To Confirm Email: " + callback
                                   };
            _emailService.SendEmailAsync(sendEmailCommand);
        }


        public async Task<UserDetailsModel> GetUserByUserIdAsync(ByIdQuery query)
        {
            ArgumentNullException.ThrowIfNull(query);
            ArgumentException.ThrowIfNullOrEmpty(query.UserId);

            var user = await _context.Query<IUserDbQuery>().WithParams(query.UserId)
                                     .ExecuteAsync(query.CancellationToken);
            CheckUserExists(user);

            return _mapper.Map<UserDetailsModel>(user);
        }


        public async Task<UserLoginResponse> LoginUserAsync(UserLoginCommand command)
        {
            ArgumentNullException.ThrowIfNull(command);

            var user = await _context.Query<IUserByNameOrEmailDbQuery>()
                                     .WithParams(command.Email)
                                     .ExecuteAsync(command.CancellationToken);
            CheckUserExists(user);

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

                throw new DomainException(typeof(User), "Account is Now Locked. Follow Instructions Sent To Email Address");
            }
        }

        public async Task ResetUserPasswordAsync(UserResetPasswordCommand command)
        {
            ArgumentNullException.ThrowIfNull(command);

            var user = await _context.Query<IUserByNameOrEmailDbQuery>()
                                     .WithParams(command.Email)
                                     .Include(new[] { nameof(UserSetting) })
                                     .ExecuteAsync(command.CancellationToken);
            CheckUserExists(user);
            await _identityManager.ResetPasswordAsync(user, command.Token, command.Password);

            user.UserSetting.ResetPasswordLinkSent = false;
            user.UserSetting.ResetPasswordLinkSentRecord = null;
            await _context.SaveChangesAsync(command.CancellationToken);
        }

        public async Task SendResetPasswordAsync(UserForgotPasswordCommand command)
        {
            ArgumentNullException.ThrowIfNull(command);

            var user = await _context.Query<IUserByNameOrEmailDbQuery>()
                                     .WithParams(command.Email)
                                     .Include(new[] { nameof(UserSetting) })
                                     .ExecuteAsync(command.CancellationToken);
            CheckUserExists(user);

            // Stop spamming of reset password email
            if (user.UserSetting.ResetPasswordLinkSent && user.UserSetting.ResetPasswordLinkSentRecord.HasValue)
            {
                if (DateTimeOffset.Now < user.UserSetting.ResetPasswordLinkSentRecord.Value.Add(_cooldownMinutes))
                {
                    throw new DomainException(typeof(User), "reset password link already sent");
                }
            }

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

            _emailService.SendEmailAsync(sendEmailCommand);

            user.UserSetting.ResetPasswordLinkSent = true;
            user.UserSetting.ResetPasswordLinkSentRecord = DateTimeOffset.UtcNow;

            await _context.SaveChangesAsync(command.CancellationToken);
        }

        #endregion
    }
}
