using EvolutionOfCreatures.Logic.Players;
using System;

namespace EvolutionOfCreatures.Logic.Accounts
{
    public class AccountDto
    {
        public Guid Id { get; set; }


        public DateTime CreatedAt { get; set; }


        public PlayerDto Player { get; set; }
    }
}
