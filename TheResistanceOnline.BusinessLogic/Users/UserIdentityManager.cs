using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TheResistanceOnline.BusinessLogic.Emails;
using TheResistanceOnline.Data.Exceptions;
using TheResistanceOnline.Data.Users;

namespace TheResistanceOnline.BusinessLogic.Users
{
    public interface IUserIdentityManager
    {
        Task ConfirmUsersEmailAsync(User user, string? token);

        Task<string> CreateIdentityAsync(User user, string? password);

        Task CreateUserRoleAsync(User user, string role);

        JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials);

        Task<string> GetPasswordResetTokenAsync(User user);

        SigningCredentials GetSigningCredentials();

        Task<string> LoginUserByEmailAsync(User user, string? password);

        Task ResetPasswordAsync(User user, string? token, string? newPassword);
    }

    public class UserIdentityManager: IUserIdentityManager
    {
        #region Fields

        private readonly IEmailService _emailService;

        private readonly IConfigurationSection _jwtSettings;

        private readonly UserManager<User> _userManager;

        #endregion

        #region Construction

        public UserIdentityManager(UserManager<User> userManager, IConfiguration configuration, IEmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;

            _jwtSettings = configuration.GetSection("JwtSettings");
        }

        #endregion

        #region Private Methods

        private async Task<User> FindUserByEmailAsync(User user)
        {
            var foundUser = await _userManager.FindByEmailAsync(user.Email);

            if (foundUser == null)
            {
                throw new DomainException(typeof(User), user.Email, "Email Not Found");
            }

            return foundUser;
        }

        #endregion

        #region Public Methods

        public async Task ConfirmUsersEmailAsync(User user, string? token)
        {
            var foundUser = await FindUserByEmailAsync(user);

            var confirmResult = await _userManager.ConfirmEmailAsync(foundUser, token);
            if (!confirmResult.Succeeded)
            {
                throw new DomainException(typeof(User), user.Email, "Invalid Email Confirmation Request");
            }
        }

        // returns email token confirmation
        public async Task<string> CreateIdentityAsync(User user, string? password)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                var description = result.Errors.FirstOrDefault()?.Description;
                if (description != null)
                {
                    throw new DomainException(typeof(User), user.UserName, description);
                }
            }

            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task CreateUserRoleAsync(User user, string role)
        {
            await _userManager.AddToRoleAsync(user, role);
        }

        public JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials)
        {
            return new JwtSecurityToken(_jwtSettings["validIssuer"],
                                        _jwtSettings["validAudience"],
                                        expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtSettings["expiryInMinutes"])),
                                        signingCredentials: signingCredentials);
        }

        public async Task<string> GetPasswordResetTokenAsync(User user)
        {
            var foundUser = await FindUserByEmailAsync(user);
            return await _userManager.GeneratePasswordResetTokenAsync(foundUser);
        }

        public SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(_jwtSettings.GetSection("securityKey").Value);
            var secret = new SymmetricSecurityKey(key);

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        public async Task<string> LoginUserByEmailAsync(User user, string? password)
        {
            var foundUser = await FindUserByEmailAsync(user);
            var confirmedEmail = await _userManager.IsEmailConfirmedAsync(foundUser);
            if (!confirmedEmail)
            {
                throw new DomainException(typeof(User), user.Email, "Please confirm email address");
            }

            var passwordCheck = await _userManager.CheckPasswordAsync(foundUser, password);
            if (!passwordCheck)
            {
                await _userManager.AccessFailedAsync(foundUser); 
                var accountIsLocked = await _userManager.IsLockedOutAsync(foundUser);
                if (accountIsLocked)
                {
                    throw new UnauthorizedAccessException();
                }
                throw new DomainException(typeof(User), password, "Incorrect password");
            }
            
            await _userManager.ResetAccessFailedCountAsync(user);
            var signingCredentials = GetSigningCredentials();
            var tokenOptions = GenerateTokenOptions(signingCredentials);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return token;
        }

        public async Task ResetPasswordAsync(User user, string? token, string? newPassword)
        {
            var foundUser = await FindUserByEmailAsync(user);
            // Use this for if user is locked out and expiry is not over yet
            await _userManager.SetLockoutEndDateAsync(foundUser, null);
            await _userManager.ResetPasswordAsync(foundUser, token, newPassword);

        }

        #endregion
    }
}
