using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using TheResistanceOnline.BusinessLogic.Settings;
using TheResistanceOnline.BusinessLogic.Users.Models;
using TheResistanceOnline.Data.Exceptions;
using TheResistanceOnline.Data.Users;
using TheResistanceOnline.Data.UserSettings;

namespace TheResistanceOnline.BusinessLogic.Users
{
    public interface IUserIdentityManager
    {
        Task ConfirmUsersEmailAsync([NotNull] User user, [NotNull] string token);

        Task<string> CreateIdentityAsync([NotNull] User user, [NotNull] string password);

        Task CreateUserRoleAsync([NotNull] User user, [NotNull] string role);

        Task<string> GetPasswordResetTokenAsync([NotNull] User user);

        Task<UserLoginResponse> LoginUserByEmailAsync([NotNull] User user, [NotNull] string password);

        Task ResetPasswordAsync([NotNull] User user, [NotNull] string token, [NotNull] string newPassword);
    }

    public class UserIdentityManager: IUserIdentityManager
    {
        #region Fields

        private readonly ISettingsService _settings;

        private readonly UserManager<User> _userManager;

        #endregion

        #region Construction

        public UserIdentityManager(UserManager<User> userManager, ISettingsService settings)
        {
            _userManager = userManager;
            _settings = settings;
        }

        #endregion

        #region Private Methods

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            return new JwtSecurityToken(_settings.GetAppSettings().JWTValidIssuer,
                                        _settings.GetAppSettings().JWTValidAudience,
                                        expires: DateTime.Now.AddMinutes(Convert.ToDouble(_settings.GetAppSettings().JWTExpiryInMinutes)),
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
            var key = Encoding.UTF8.GetBytes(_settings.GetAppSettings().JWTSecurityKey);
            var secret = new SymmetricSecurityKey(key);

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        #endregion

        #region Public Methods

        public async Task ConfirmUsersEmailAsync(User user, string token)
        {
            var confirmResult = await _userManager.ConfirmEmailAsync(user, token);
            if (!confirmResult.Succeeded)
            {
                throw new DomainException(typeof(User), user.Email, "Invalid Email Confirmation Request");
            }
        }

        // returns email token confirmation
        public async Task<string> CreateIdentityAsync(User user, string password)
        {
            var userSetting = new UserSetting();
            user.UserSetting = userSetting;

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                var description = result.Errors.FirstOrDefault()?.Description;
                if (description != null)
                {
                    throw new DomainException(typeof(User), user.UserName, description);
                }
            }

            // added this for signalR????
            await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Name, user.UserName));

            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task CreateUserRoleAsync(User user, string role)
        {
            await _userManager.AddToRoleAsync(user, role);
        }

        public async Task<string> GetPasswordResetTokenAsync(User user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<UserLoginResponse> LoginUserByEmailAsync(User user, string password)
        {
            var confirmedEmail = await _userManager.IsEmailConfirmedAsync(user);
            if (!confirmedEmail)
            {
                throw new DomainException(typeof(User), user.Email, "Please confirm email address");
            }

            var accountLocked = await _userManager.IsLockedOutAsync(user);
            if (accountLocked)
            {
                throw new DomainException(typeof(User), "Account Is Locked, Please see your Emails for instructions to reset your password");
            }

            var passwordCheck = await _userManager.CheckPasswordAsync(user, password);
            if (!passwordCheck)
            {
                await _userManager.AccessFailedAsync(user);
                var accountIsLocked = await _userManager.IsLockedOutAsync(user);
                if (accountIsLocked)
                {
                    throw new UnauthorizedAccessException();
                }

                throw new DomainException(typeof(User), password, "Incorrect password");
            }

            await _userManager.ResetAccessFailedCountAsync(user);
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims(user);
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return new UserLoginResponse
                   {
                       Token = token,
                       UserId = user.Id
                   };
        }

        public async Task ResetPasswordAsync(User user, string token, string newPassword)
        {
            // Use this for if user is locked out and expiry is not over yet
            await _userManager.SetLockoutEndDateAsync(user, null);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            if (!result.Succeeded)
            {
                var description = result.Errors.FirstOrDefault()?.Description;
                if (description != null)
                {
                    throw new DomainException(typeof(User), user.UserName, description);
                }
            }
        }

        #endregion
    }
}
