using OpenIddict.EntityFrameworkCore.Models;

namespace TheResistanceOnline.Data.Entities;

public class Application: OpenIddictEntityFrameworkCoreApplication<int, Authorization, Token>
{
}
