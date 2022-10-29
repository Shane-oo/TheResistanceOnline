using Microsoft.Extensions.Caching.Memory;
using TheResistanceOnline.BusinessLogic.Core.Queries;
using TheResistanceOnline.BusinessLogic.Users.Commands;
using TheResistanceOnline.BusinessLogic.Users.Models;
using TheResistanceOnline.Data.Users;

namespace TheResistanceOnline.BusinessLogic.Users;

public class CachedUserService: IUserService
{
    #region Fields
    
    private readonly UserService _decoratedUserService;

    private readonly IMemoryCache _memoryCache;

    #endregion

    #region Construction

    public CachedUserService(UserService decoratedUserService, IMemoryCache memoryCache)
    {
        _decoratedUserService = decoratedUserService;
        _memoryCache = memoryCache;
    }

    #endregion

    #region Public Methods

    public Task ConfirmUserEmailAsync(UserConfirmEmailCommand command)
    {
        return _decoratedUserService.ConfirmUserEmailAsync(command);
    }

    public Task CreateUserAsync(UserRegisterCommand command)
    {
        return _decoratedUserService.CreateUserAsync(command);
    }

    public Task<User> GetUserByEmailOrNameAsync(ByIdAndNameQuery query)
    {
        var key = $"GetUserByEmailOrName-{query}";
        return _memoryCache.GetOrCreateAsync(key, entry =>
                                                  {
                                                      entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
                                                      return _decoratedUserService.GetUserByEmailOrNameAsync(query);
                                                  });
    }

    public Task<UserDetailsModel> GetUserByIdAsync(ByIdQuery query)
    {
        var key = $"GetUserById-{query}";

        return _memoryCache.GetOrCreateAsync(key, entry =>
                                                  {
                                                      entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
                                                      return _decoratedUserService.GetUserByIdAsync(query);
                                                  });
    }

    public Task<UserLoginResponse> LoginUserAsync(UserLoginCommand command)
    {
        return _decoratedUserService.LoginUserAsync(command);
    }

    public Task ResetUserPasswordAsync(UserResetPasswordCommand command)
    {
        return _decoratedUserService.ResetUserPasswordAsync(command);
    }

    public Task SendResetPasswordAsync(UserForgotPasswordCommand command)
    {
        return _decoratedUserService.SendResetPasswordAsync(command);
    }

    #endregion
}
