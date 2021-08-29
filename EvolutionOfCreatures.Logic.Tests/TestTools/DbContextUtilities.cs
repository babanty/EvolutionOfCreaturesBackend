using Microsoft.EntityFrameworkCore;

namespace EvolutionOfCreatures.Logic.Tests.TestTools
{
    public static class DbContextUtilities
    {
        public static DbContextOptions<TDbContext> GetInMemoryDbOptions<TDbContext>() where TDbContext : DbContext
        {
            var builder = new DbContextOptionsBuilder<TDbContext>();
            builder.UseInMemoryDatabase(nameof(TDbContext));
            return builder.Options;
        }
    }
}
