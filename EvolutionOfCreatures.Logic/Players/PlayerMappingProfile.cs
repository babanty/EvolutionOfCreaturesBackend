using AutoMapper;
using EvolutionOfCreatures.Db.Entities;
using EvolutionOfCreatures.Logic.Players.Dtos;

namespace EvolutionOfCreatures.Logic.Players
{
    public class PlayerMappingProfile : Profile
    {
        public PlayerMappingProfile()
        {
            CreateMap<Player, PlayerDto>();
            
            CreateMap<PlayerProgress, PlayerProgressDto>();
            
            CreateMap<PlayerSettings, PlayerSettingsDto>();

            CreateMap<PlayerStatistics, PlayerStatisticsDto>();
        }
    }
}
