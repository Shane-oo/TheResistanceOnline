using AutoMapper;
using TheResistanceOnline.BusinessLogic.Users.Models;
using TheResistanceOnline.Data.Users;

namespace TheResistanceOnline.BusinessLogic.Users
{

    public class UserMappingProfile:Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserDetails>();
               // .ForMember(c => c.FullAddress,
                  //         opt => opt.MapFrom(x => string.Join(' ', x.Address, x.Country)));
        }
    }
}