using AutoMapper;
using TheResistanceOnline.BusinessLogic.Games.Models;
using TheResistanceOnline.Data.Games;

namespace TheResistanceOnline.BusinessLogic.Games;

public class GameMappingProfile: Profile
{
    #region Construction

    public GameMappingProfile()
    {
        CreateMap<PlayerVariablesModel, PlayerValue>().ForMember(pv => pv.Team, opt => opt.MapFrom(pvm => (int)pvm.Team));
        CreateMap<PlayerValue, PlayerVariablesModel>().ForMember(pv => pv.Team, opt => opt.MapFrom(pvm => (TeamModel)pvm.Team));
    }

    #endregion
}

