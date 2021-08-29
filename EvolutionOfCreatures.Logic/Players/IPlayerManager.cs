using System.Threading.Tasks;
using EvolutionOfCreatures.Db.Entities;

namespace EvolutionOfCreatures.Logic.Players
{
    public interface IPlayerManager
    {
        Task<Player> CreateEntity(CreatePlayerRequest request);
    }
}
