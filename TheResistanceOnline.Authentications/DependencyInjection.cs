using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TheResistanceOnline.Authentications.ExternalIdentities;

namespace TheResistanceOnline.Authentications;

public static class DependencyInjection
{
    #region Public Methods

    public static void AddAuthenticationDbQueries(this IServiceCollection services)
    {
        services.AddScoped<IMicrosoftUserByObjectIdDbQuery, MicrosoftUserByObjectIdDbQuery>();
        services.AddScoped<IGoogleUserBySubjectDbQuery, GoogleUserBySubjectDbQuery>();
        services.AddScoped<IRedditUserByIdDbQuery, RedditUserByIdDbQuery>();
    }

    public static void AddAuthenticationServices(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
        services.AddValidatorsFromAssembly(assembly);
    }

    #endregion
}
