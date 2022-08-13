using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
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

        Task<string> GetPasswordResetTokenAsync(User user);

        Task<string> LoginUserByEmailAsync(User user, string? password);

        Task ResetPasswordAsync(User user, string? token, string? newPassword);
    }

    public class UserIdentityManager: IUserIdentityManager
    {
        #region Fields

        private readonly IEmailService _emailService;

        private static readonly string? _expiryInMinutes = Environment.GetEnvironmentVariable("ExpiryInMinutes");
        private static readonly string? _securityKey = Environment.GetEnvironmentVariable("SecurityKey");

        private readonly UserManager<User> _userManager;
        private static readonly string? _validAudience = Environment.GetEnvironmentVariable("ValidAudience");
        private static readonly string? _validIssuer = Environment.GetEnvironmentVariable("ValidIssuer");

        #endregion

        #region Construction

        public UserIdentityManager(UserManager<User> userManager, IEmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;

            if (_validIssuer == null || _validAudience == null || _securityKey == null || _expiryInMinutes == null)
            {
                throw new NullReferenceException("JWT settings not found UserIdentityManager");
            }
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

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            return new JwtSecurityToken(_validIssuer,
                                        _validAudience,
                                        expires: DateTime.Now.AddMinutes(Convert.ToDouble(_expiryInMinutes)),
                                        signingCredentials: signingCredentials,
                                        claims: claims);
        }

        private async Task<List<Claim>> GetClaims(User user)
        {
            var claims = new List<Claim>
                         {
                             new Claim(ClaimTypes.Name, user.Email)
                         };

            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            return claims;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(_securityKey);
            var secret = new SymmetricSecurityKey(key);

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
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

        public async Task<string> GetPasswordResetTokenAsync(User user)
        {
            var foundUser = await FindUserByEmailAsync(user);
            return await _userManager.GeneratePasswordResetTokenAsync(foundUser);
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

            await _userManager.ResetAccessFailedCountAsync(foundUser);
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims(foundUser);
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
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
