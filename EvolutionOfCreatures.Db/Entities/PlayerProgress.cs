using Infrastructure.Tools.Db;
using System;

namespace EvolutionOfCreatures.Db.Entities
{
    public class PlayerProgress : IUniqueIdentifiableEntity, ICanBeEditedOffline
    {
        public Guid Id { get; set; }


        public int Version { get; set; }


        public Guid PlayerId { get; set; }


        /// <summary> The maximum level reached by the player </summary>
        public int MaxLevel { get; set; }
    }
}
