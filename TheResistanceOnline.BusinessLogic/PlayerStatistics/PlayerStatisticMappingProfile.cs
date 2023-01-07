using AutoMapper;
using TheResistanceOnline.BusinessLogic.PlayerStatistics.Models;
using TheResistanceOnline.Data.PlayerStatistics;

namespace TheResistanceOnline.BusinessLogic.PlayerStatistics;

public class PlayerStatisticMappingProfile: Profile
{
    #region Construction

    public PlayerStatisticMappingProfile()
    {
        CreateMap<IEnumerable<PlayerStatistic>, PlayerStatisticDetailsModel>()
            .ForMember(dest => dest.ResistanceWins, opt => opt.MapFrom(playerStatistics => playerStatistics.Count(ps => ps.Won)))
            .ForMember(dest => dest.SpyWins, opt => opt.MapFrom(playerStatistics => playerStatistics.Count(ps => !ps.Won)));
    }

    #endregion
}
