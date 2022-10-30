using Microsoft.EntityFrameworkCore;
using TheResistanceOnline.BusinessLogic.Users.DbQueries;
using TheResistanceOnline.Data.Users;

namespace TheResistanceOnline.Infrastructure.Data.Queries.Users
{
    public class UserDbQuery: IUserDbQuery
    {
        #region Fields

        private readonly Context _context;
        private string? _userId;

        #endregion

        #region Construction

        public UserDbQuery(Context context)
        {
            _context = context;
        }

        #endregion

        #region Public Methods

        public async Task<User> ExecuteAsync(CancellationToken cancellationToken)
        {
            var user = await _context.Users
                                     .Include(p => p.ProfilePicture)
                                     .Include(ud => ud.DiscordUser)
                                     .Include(us => us.UserSetting)
                                     .FirstAsync(u => u.Id == _userId,
                                                 cancellationToken);
            return user;
        }

        public IUserDbQuery WithParams(string userId)
        {
            _userId = userId;
            return this;
        }

        #endregion
    }
}
