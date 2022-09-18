using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TheResistanceOnline.BusinessLogic.Users.DbQueries;
using TheResistanceOnline.BusinessLogic.Users.Models;

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

    public async Task<UserDetailsModel> ExecuteAsync(CancellationToken cancellationToken)
    {
        var user = await _context.Users.AsNoTracking() // not sure if should have AsNoTracking()
                                 .Include(p => p.ProfilePicture)
                                 .Include(ud => ud.DiscordUser)
                                 .FirstOrDefaultAsync(u => u.Email == _userNameOrEmail || u.UserName == _userNameOrEmail,
                                                      cancellationToken);
        return _mapper.Map<UserDetailsModel>(user);
    }

    public IUserByNameOrEmailDbQuery WithParams(string nameOrEmail)
    {
        _userNameOrEmail = nameOrEmail;
        return this;
    }

    #endregion
}
