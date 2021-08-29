using EvolutionOfCreatures.Db.Enums;
using Infrastructure.Tools.Db;
using System;

namespace EvolutionOfCreatures.Db.Entities
{
    public class PlayerSettings : IUniqueIdentifiableEntity
    {
        public Guid Id { get; set; }


        public Guid PlayerId { get; set; }


        /// <summary> The level of errors that are sent to the server from the player's devices. </summary>
        public PlayerLogLevel LogLevel { get; set; }


        /// <summary> Fallback URLs to connect to the backend </summary>
        public string[] FallbackUrls => DefaultFallbackUrls;


        /// <summary> If the current version of the client API is less than the one specified here, then the client will need to be updated </summary>
        public int MinApiVersion => MinApiVersionForEveryone;


        private const int MinApiVersionForEveryone = 1;

        private readonly string[] DefaultFallbackUrls = new string[0];
    }
}
