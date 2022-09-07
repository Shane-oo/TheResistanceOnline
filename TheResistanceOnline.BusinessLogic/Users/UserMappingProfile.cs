using AutoMapper;
using TheResistanceOnline.BusinessLogic.Users.Models;
using TheResistanceOnline.Data.Users;

namespace TheResistanceOnline.BusinessLogic.Users
{
    public class UserMappingProfile: Profile
    {
        #region Construction

        public UserMappingProfile()
        {

            CreateMap<User, UserDetailsModel>().ForMember(u => u.UserId, opt => opt.MapFrom(u => u.Id));
            // .ForMember(c => c.FullAddress,
            //         opt => opt.MapFrom(x => string.Join(' ', x.Address, x.Country)));
        }

        #endregion
    }
}
