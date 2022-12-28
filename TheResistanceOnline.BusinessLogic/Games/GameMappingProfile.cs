using AutoMapper;
using TheResistanceOnline.BusinessLogic.Games.Models;
using TheResistanceOnline.Data.Games;

namespace TheResistanceOnline.BusinessLogic.Games;

public class GameMappingProfile: Profile
{
    public GameMappingProfile()
    {
        CreateMap<PlayerVariablesModel, PlayerValue>().ForMember(pv => pv.Team, opt => opt.MapFrom(pvm => (int)pvm.Team));
    }
}
