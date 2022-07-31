using JetBrains.Annotations;
using TheResistanceOnline.BusinessLogic.Users.Commands;
using TheResistanceOnline.Data.Users;

namespace TheResistanceOnline.BusinessLogic.Users
{
    public interface IUserService
    {
        Task CreateUserAsync([NotNull] UserRegisterCommand command);
    }


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

        #endregion
    }
}
