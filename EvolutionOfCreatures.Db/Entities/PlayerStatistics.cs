using Infrastructure.Tools.Db;
using System;

namespace EvolutionOfCreatures.Db.Entities
{
    public class PlayerStatistics : IUniqueIdentifiableEntity, ICanBeEditedOffline
    {
        public Guid Id { get; set; }


        public Guid PlayerId { get; set; }


        public int Version { get; set; }


        public int OfflineGameCount { get; set; }


        public int OnlineGameCount { get; set; }


        public int OfflineWinCount { get; set; }


        public int OnlineWinCount { get; set; }
    }
}
