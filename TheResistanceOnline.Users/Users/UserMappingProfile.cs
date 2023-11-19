using AutoMapper;
using TheResistanceOnline.Data.Entities;


namespace TheResistanceOnline.Users.Users;

public class UserMappingProfile: Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, UserDetailsModel>();
    }
}
