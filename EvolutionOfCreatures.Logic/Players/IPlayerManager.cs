using EvolutionOfCreatures.Db.Entities;
using System.Threading.Tasks;

namespace EvolutionOfCreatures.Logic.Players
{
    public interface IPlayerManager
    {
        Task<Player> CreateEntity(CreatePlayerRequest request);
    }
}
