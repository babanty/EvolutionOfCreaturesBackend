using Infrastructure.Tools.Db;
using System;


namespace EvolutionOfCreatures.Db.Entities
{
    public class Player : IUniqueIdentifiableEntity, ICreatedEntity
    {
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public Guid AccountId { get; set; }

        public int Rating { get; set; }

        public PlayerSettings IndividualSettings { get; set; }

        public PlayerStatistics PlayerStatistics { get; set; }
    }
}
