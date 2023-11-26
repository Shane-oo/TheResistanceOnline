using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TheResistanceOnline.Hubs.Lobbies;
using TheResistanceOnline.Hubs.Resistance;
using TheResistanceOnline.Hubs.Streams;

namespace TheResistanceOnline.Hubs;

public static class DependencyInjection
{
    #region Public Methods

    // public static void AddHubDbQueries(this IServiceCollection services)
    // {
    //     
    // }

    public static void AddHubServices(this IServiceCollection services)
    {
        services.AddSingleton<LobbyHubPersistedProperties>();
        services.AddSingleton<StreamHubPersistedProperties>();
        services.AddSingleton<ResistanceHubPersistedProperties>();

        var assembly = typeof(DependencyInjection).Assembly;
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
        services.AddValidatorsFromAssembly(assembly);
    }

    #endregion
}
