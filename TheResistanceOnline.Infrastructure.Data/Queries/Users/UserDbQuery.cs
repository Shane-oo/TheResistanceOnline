using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TheResistanceOnline.BusinessLogic.Users.DbQueries;
using TheResistanceOnline.Data.Users;

namespace TheResistanceOnline.Infrastructure.Data.Queries.Users
{
    public class UserDbQuery: IUserDbQuery
    {
        #region Fields

        private readonly Context _context;
        private readonly IMapper _mapper;
        private string? _userId;

        #endregion

        #region Construction

        public UserDbQuery(Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #endregion

        #region Public Methods

        public async Task<User> ExecuteAsync(CancellationToken cancellationToken)
        {
            var user = await _context.Users // not sure if should have AsNoTracking()
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
