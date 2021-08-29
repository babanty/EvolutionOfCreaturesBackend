using AutoMapper;
using EvolutionOfCreatures.Db.Entities;

namespace EvolutionOfCreatures.Logic.Accounts
{
    public class AccountMappingProfile : Profile
    {
        public AccountMappingProfile()
        {
            CreateMap<Account, AccountDto>();
        }
    }
}
