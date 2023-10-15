using OpenIddict.EntityFrameworkCore.Models;

namespace TheResistanceOnline.Data.Entities.AuthorizationEntities;

public class Application: OpenIddictEntityFrameworkCoreApplication<int, Authorization, Token>
{
}
