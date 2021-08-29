using System;

namespace EvolutionOfCreatures.Logic.Players
{
    public class CreatePlayerRequest
    {
        public Guid AccountId { get; set; }
        public string PlayerName { get; set; }
    }
}
