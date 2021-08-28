using Microsoft.EntityFrameworkCore;


namespace EvolutionOfCreatures.Db
{
    public class EvolutionOfCreaturesContext : DbContext
    {
        public EvolutionOfCreaturesContext(DbContextOptions<EvolutionOfCreaturesContext> options) : base(options)
        {
        }
    }
}
