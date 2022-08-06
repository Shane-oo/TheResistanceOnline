using System.IdentityModel.Tokens.Jwt;
using System.Text;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TheResistanceOnline.BusinessLogic.Users.Commands;
using TheResistanceOnline.Data.Users;

namespace TheResistanceOnline.BusinessLogic.Users
{
    public interface IUserService
    {
        Task CreateUserAsync([NotNull] UserRegisterCommand command);

 

        Task<string> LoginUserAsync([NotNull] UserLoginCommand command);
    }

    /*
     * Service for managing both User Functions for login and registration
     * and for managing jwt tokens
     */
    public class UserService: IUserService
    {
        #region Fields

        private readonly IUserIdentityManager _identityManager;

        #endregion

        #region Construction

        public UserService(IUserIdentityManager identityManager)
        {
            _identityManager = identityManager;
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

           return await _identityManager.LoginUserByEmailAsync(user,command.Password);
        }

        #endregion
    }
}
