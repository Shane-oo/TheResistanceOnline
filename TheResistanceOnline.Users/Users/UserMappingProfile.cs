using AutoMapper;
using TheResistanceOnline.Data.Entities.UserEntities;
using TheResistanceOnline.Users.Users.GetUser;

namespace TheResistanceOnline.Users.Users;

public class UserMappingProfile: Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, UserDetailsModel>();
    }
}
