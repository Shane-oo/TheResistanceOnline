using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TheResistanceOnline.BusinessLogic.Users.DbQueries;
using TheResistanceOnline.Data.Users;

namespace TheResistanceOnline.Infrastructure.Data.Queries.Users;

public class UserByNameOrEmailDbQuery: IUserByNameOrEmailDbQuery
{
    #region Fields

    private readonly Context _context;
    private readonly IMapper _mapper;
    private string? _userNameOrEmail;

    #endregion

    #region Construction

    public UserByNameOrEmailDbQuery(Context context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    #endregion

    #region Public Methods

    public async Task<User?> ExecuteAsync(CancellationToken cancellationToken)
    {
        var user = await _context.Users
                                 .Include(p => p.ProfilePicture)
                                 .Include(ud => ud.DiscordUser)
                                 .Include(us => us.UserSetting)
                                 .FirstOrDefaultAsync(u => u.Email == _userNameOrEmail || u.UserName == _userNameOrEmail,
                                                      cancellationToken);
        return user;
    }

    public IUserByNameOrEmailDbQuery WithParams(string nameOrEmail)
    {
        _userNameOrEmail = nameOrEmail;
        return this;
    }

    #endregion
}
