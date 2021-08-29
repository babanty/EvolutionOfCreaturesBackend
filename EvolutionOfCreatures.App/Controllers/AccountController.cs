using EvolutionOfCreatures.Logic.Accounts;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace EvolutionOfCreatures.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountManager _entityManager;

        public AccountController(IAccountManager entityManager)
        {
            _entityManager = entityManager;
        }


        [HttpPost]
        public async Task<AccountDto> Create(CreateAccountRequest request) => await _entityManager.Create(request);
    }
}
