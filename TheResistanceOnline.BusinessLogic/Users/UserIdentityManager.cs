using Microsoft.AspNetCore.Identity;
using TheResistanceOnline.Data.Exceptions;
using TheResistanceOnline.Data.Users;

namespace TheResistanceOnline.BusinessLogic.Users
{
    public interface IUserIdentityManager
    {
        Task CreateIdentityAsync(User user, string password);
    }

    public class UserIdentityManager: IUserIdentityManager
    {
        #region Fields

        private readonly UserManager<User> _userManager;

        #endregion

        #region Construction

        public UserIdentityManager(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        #endregion

        #region Public Methods

        public async Task CreateIdentityAsync(User user, string password)
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
        }

        #endregion
    }
}
