using EvolutionOfCreatures.Db.Enums;
using Infrastructure.Tools.Db;
using System;

namespace EvolutionOfCreatures.Db.Entities
{
    public class PlayerSettings : IUniqueIdentifiableEntity
    {
        public Guid Id { get; set; }


        public Guid PlayerId { get; set; }


        /// <summary> Уровень ошибок которые отправлять на сервер с устройств игрока </summary>
        public PlayerLogLevel LogLevel { get; set; }


        /// <summary> Запасные урлы по которым подключаться к беку </summary>
        public string[] FallbackUrls => new string[0];


        /// <summary> Если текущая версия API (зашитая константа GameWebClient.ApiVersion) меньше чем указанная тут, то потребует обновления </summary>
        public int MinApiVersion => 1;
    }
}
