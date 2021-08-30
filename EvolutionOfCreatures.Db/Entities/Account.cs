using Infrastructure.Tools.Db;
using System;
using System.Collections.Generic;

namespace EvolutionOfCreatures.Db.Entities
{
    public class Account : IUniqueIdentifiableEntity, ICreatedEntity
    {
        public Guid Id { get; set; }

        
        public DateTime CreatedAt { get; set; }


        /// <summary> Trade transaction IDs </summary>
        public ICollection<string> TransactionIds { get; set; }

        
        public Player Player { get; set; }
    }
}
