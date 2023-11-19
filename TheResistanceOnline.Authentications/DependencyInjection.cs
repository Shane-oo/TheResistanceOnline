using Microsoft.Extensions.DependencyInjection;
using TheResistanceOnline.Authentications.ExternalIdentities;

namespace TheResistanceOnline.Authentications;

public static class DependencyInjection
{
    #region Public Methods

    public static void AddAuthenticationDbQueries(this IServiceCollection services)
    {
        services.AddTransient<IMicrosoftUserByObjectIdDbQuery, MicrosoftUserByObjectIdDbQuery>();
        services.AddTransient<IGoogleUserBySubjectDbQuery, GoogleUserBySubjectDbQuery>();
    }

    public static void AddAuthenticationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
    }

    #endregion
}
