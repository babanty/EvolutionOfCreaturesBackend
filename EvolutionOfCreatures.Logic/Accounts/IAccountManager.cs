using System.Threading.Tasks;

namespace EvolutionOfCreatures.Logic.Accounts
{
    public interface IAccountManager
    {
        Task<AccountDto> Create(CreateAccountRequest request);
    }
}
