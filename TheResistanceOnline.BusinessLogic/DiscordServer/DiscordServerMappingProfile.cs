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
        }

        #endregion
    }
}
