using Microsoft.AspNetCore.Identity;

namespace TheResistanceOnline.Data.Users;

public class User :IdentityUser
{
    public string? UserName { get; set; }
}
