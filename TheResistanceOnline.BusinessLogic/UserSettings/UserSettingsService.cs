using TheResistanceOnline.BusinessLogic.Users.DbQueries;
using TheResistanceOnline.BusinessLogic.UserSettings.Commands;
using TheResistanceOnline.Data;
using TheResistanceOnline.Data.Exceptions;
using TheResistanceOnline.Data.Users;

namespace TheResistanceOnline.BusinessLogic.UserSettings
{
    public interface IUserSettingsService
    {
        Task UpdateUserSettingsAsync(UserSettingsUpdateCommand command);
    }

    public class UserSettingsService: IUserSettingsService
    {
        #region Fields

        private readonly IDataContext _context;

        #endregion

        #region Construction

        public UserSettingsService(IDataContext context)
        {
            _context = context;
        }

        #endregion

        #region Public Methods

        public async Task UpdateUserSettingsAsync(UserSettingsUpdateCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            var user = await _context.Query<IUserDbQuery>().WithParams(command.UserId)
                                     .ExecuteAsync(command.CancellationToken);
            if (user == null)
            {
                throw new DomainException(typeof(User), "User Not Found");
            }

            //if(command.updateSetting){do}
            // Old example
            // if (command.UpdateUserWantsToUseDiscord)
            // {
            //     user.UserSetting.UserWantsToUseDiscord = command.UserWantsToUseDiscord;
            //     user.UserSetting.UserWantsToUseDiscordRecord = DateTimeOffset.Now;
            // }

            await _context.SaveChangesAsync(command.CancellationToken);
        }

        #endregion
    }
}
