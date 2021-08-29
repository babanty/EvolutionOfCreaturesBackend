using Infrastructure.Tools.Db;
using System;

namespace EvolutionOfCreatures.Db.Entities
{
    public class PlayerStatistics : IUniqueIdentifiableEntity, ICanBeEditedOffline
    {
        public Guid Id { get; set; }


        public Guid PlayerId { get; set; }


        public int Version { get; set; }


        /// <summary> Количество офлайн игр </summary>
        public int OfflineGameCount { get; set; }


        /// <summary> Количество онлайн игр </summary>
        public int OnlineGameCount { get; set; }


        /// <summary> Количество офлайн побед </summary>
        public int OfflineWinCount { get; set; }


        /// <summary> Количество онлайн побед </summary>
        public int OnlineWinCount { get; set; }
    }
}
