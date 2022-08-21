using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TheResistanceOnline.BusinessLogic.Users.DbQueries;
using TheResistanceOnline.BusinessLogic.Users.Models;

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

        public async Task<UserDetailsModel> ExecuteAsync(CancellationToken cancellationToken)
        {
            var user = await _context.Users.AsNoTracking() // not sure if should have AsNoTracking()
                                     .Include(p => p.ProfilePicture)
                                     .FirstOrDefaultAsync(u => u.Id == _userId,
                                                          cancellationToken);
            return _mapper.Map<UserDetailsModel>(user);
        }

        public IUserDbQuery WithParams(string userId)
        {
            _userId = userId;
            return this;
        }

        #endregion
    }
}
