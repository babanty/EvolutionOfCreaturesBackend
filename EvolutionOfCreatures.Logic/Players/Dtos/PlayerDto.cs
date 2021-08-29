using System;

namespace EvolutionOfCreatures.Logic.Players.Dtos
{
    public class PlayerDto
    {
        public Guid Id { get; set; }

        public Guid AccountId { get; set; }

        public int Rating { get; set; }

        public PlayerSettingsDto PlayerSettings { get; set; }

        public PlayerStatisticsDto PlayerStatistics { get; set; }

        public PlayerProgressDto PlayerProgress { get; set; }
    }
}
