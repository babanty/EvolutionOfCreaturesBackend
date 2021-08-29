using EvolutionOfCreatures.Db.Enums;

namespace EvolutionOfCreatures.Logic.Players.Dtos
{
    public class PlayerSettingsDto
    {
        public PlayerLogLevel LogLevel { get; set; }


        public string[] FallbackUrls { get; set; }


        public int MinApiVersion { get; set; }
    }
}
