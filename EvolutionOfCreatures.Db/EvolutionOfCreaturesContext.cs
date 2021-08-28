using EvolutionOfCreatures.Db.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace EvolutionOfCreatures.Db
{
    public class EvolutionOfCreaturesContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }

        public DbSet<Player> Players { get; set; }

        public DbSet<PlayerSettings> ClientsSettings { get; set; }

        public DbSet<PlayerProgress> PlayersProgress { get; set; }

        public DbSet<PlayerStatistics> PlayersStatistics { get; set; }


        public EvolutionOfCreaturesContext(DbContextOptions<EvolutionOfCreaturesContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // нужно чтобы мочь сохранять массив
            builder.Entity<Account>()
                       .Property(t => t.TransactionIds)
                       .HasConversion(
                            m => JsonConvert.SerializeObject(m),
                            m => JsonConvert.DeserializeObject<string[]>(m));

            // игнорить т.к. они пока что захардкоженные
            builder.Entity<PlayerSettings>().Ignore(c => c.MinApiVersion)
                                            .Ignore(c => c.FallbackUrls);
        }

    }
}
