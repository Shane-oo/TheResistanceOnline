using TheResistanceOnline.BusinessLogic.Users;

namespace TheResistanceOnline.Web.DI;

public static class DISetup
{
    #region Public Methods

    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserIdentityManager, UserIdentityManager>();
    }

    #endregion
}