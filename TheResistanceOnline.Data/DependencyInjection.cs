using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TheResistanceOnline.Data.Entities;
using TheResistanceOnline.Data.Interceptors;
using TheResistanceOnline.Data.Queries;

namespace TheResistanceOnline.Data;

public static class DependencyInjection
{
    public static void AddDataContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<UpdateAuditableEntitiesInterceptor>();
        services.AddDbContext<Context>((sp, o) =>
                                       {
                                           var connectionString = configuration.GetConnectionString("ResistanceDb");
                                           ArgumentException.ThrowIfNullOrEmpty(connectionString);
                                           var auditableInterceptor = sp.GetService<UpdateAuditableEntitiesInterceptor>();

                                           o.UseSqlServer(connectionString)
                                            .AddInterceptors(auditableInterceptor
                                                             ?? throw new InvalidOperationException("auditableInterceptor cannot be null"));

                                           o.UseOpenIddict<Application, Authorization, Scope, Token, int>();
                                       });
        services.AddScoped<IDataContext, DataContext>();
    }

    public static void AddSharedDbQueries(this IServiceCollection services)
    {
        services.AddTransient<IUserByNameDbQuery, UserByNameDbQuery>();
        services.AddTransient<IUserByUserIdDbQuery, UserByUserIdDbQuery>();
        services.AddTransient<IRoleByIdDbQuery, RoleByIdDbQuery>();
        services.AddTransient<IRoleByNameDbQuery, RoleByNameDbQuery>();
        services.AddTransient<IUsersByRoleNamesDbQuery, UsersByRoleNamesDbQuery>();
    }
}
