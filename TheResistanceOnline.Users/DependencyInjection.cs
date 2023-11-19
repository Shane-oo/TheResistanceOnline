using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;
using TheResistanceOnline.Data.Entities;
using TheResistanceOnline.Users.Users;

namespace TheResistanceOnline.Users;

public static class DependencyInjection
{
    #region Public Methods

    public static void AddUserIdentity(this IServiceCollection services)
    {
        services.AddIdentity<User, Role>(o =>
                                         {
                                             o.ClaimsIdentity.UserNameClaimType = OpenIddictConstants.Claims.Name;
                                             o.ClaimsIdentity.UserIdClaimType = OpenIddictConstants.Claims.Subject;
                                             o.ClaimsIdentity.RoleClaimType = OpenIddictConstants.Claims.Role;
                                             o.User.RequireUniqueEmail = false; // disables user needs email to create
                                         })
                .AddUserManager<UserManager<User>>()
                .AddUserStore<UserRoleStore>()
                .AddRoleStore<IRoleStore>()
                .AddDefaultTokenProviders();
    }

    public static void AddUserServices(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
        services.AddAutoMapper(assembly);
        services.AddValidatorsFromAssembly(assembly);
    }

    #endregion
}
