using EvolutionOfCreatures.Logic.Players.Dtos;
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
