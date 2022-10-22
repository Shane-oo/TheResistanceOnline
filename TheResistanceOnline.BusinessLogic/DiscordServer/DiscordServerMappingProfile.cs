using AutoMapper;
using TheResistanceOnline.BusinessLogic.DiscordServer.Models;
using TheResistanceOnline.Data.DiscordServer;

namespace TheResistanceOnline.BusinessLogic.DiscordServer
{
    public class DiscordServerMappingProfile: Profile
    {
        #region Construction

        public DiscordServerMappingProfile()
        {
            CreateMap<DiscordUser, DiscordUserDetailsModel>();
            CreateMap<DiscordUserResponseModel, DiscordUser>().ForMember(u => u.DiscordTag,
                                                                         opt => opt.MapFrom(
                                                                                            r => r.UserName + '#' + r.Discriminator
                                                                                           ))
                                                              .ForMember(u => u.Name, opt => opt.MapFrom(r => r.UserName));
        }

        #endregion
    }
}
