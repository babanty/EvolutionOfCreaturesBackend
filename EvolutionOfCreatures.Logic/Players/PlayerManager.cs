using EvolutionOfCreatures.Db;
using EvolutionOfCreatures.Db.Entities;
using EvolutionOfCreatures.Db.Enums;
using FluentValidation;
using System;
using System.Threading.Tasks;

namespace EvolutionOfCreatures.Logic.Players
{
    public class PlayerManager : IPlayerManager
    {
        private readonly IValidator<CreatePlayerRequest> _validatorCreateRequest;
        private readonly EvolutionOfCreaturesContext _dbContext;

        private const int DefaultPlayerRating = 100;
        private const PlayerLogLevel DefaultPlayerLogLevel = PlayerLogLevel.Warning;


        public PlayerManager(EvolutionOfCreaturesContext dbContext,
                             IValidator<CreatePlayerRequest> validatorCreateRequest)
        {
            _dbContext = dbContext;
            _validatorCreateRequest = validatorCreateRequest;
        }


        public async Task<Player> CreateEntity(CreatePlayerRequest request)
        {
            await _validatorCreateRequest.ValidateAndThrowAsync(request);

            var playerId = Guid.NewGuid();

            // we create 4 related entities, for this we use a transaction in case something goes wrong, then there will be no garbage left
            Player entity;
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                var playerSettings = await CreatePlayerSettings(playerId);
                var playerStatistics = await CreatePlayerStatistics(playerId);
                var playerProgress = await CreatePlayerProgress(playerId);

                entity = new Player
                {
                    Id = playerId,
                    CreatedAt = DateTime.UtcNow,
                    AccountId = request.AccountId,
                    Name = request.PlayerName,
                    Rating = DefaultPlayerRating,
                    PlayerSettings = playerSettings,
                    PlayerStatistics = playerStatistics,
                    PlayerProgress = playerProgress
                };

                _dbContext.Add(entity);

                await _dbContext.SaveChangesAsync();

                await transaction.CommitAsync();
            }

            return entity;
        }


        private async Task<PlayerProgress> CreatePlayerProgress(Guid playerId)
        {
            var playerProgress = new PlayerProgress()
            {
                Id = Guid.NewGuid(),
                PlayerId = playerId
            };

            _dbContext.Add(playerProgress);

            await _dbContext.SaveChangesAsync();

            return playerProgress;
        }


        private async Task<PlayerSettings> CreatePlayerSettings(Guid playerId)
        {
            var playerSettings = new PlayerSettings()
            {
                Id = Guid.NewGuid(),
                LogLevel = DefaultPlayerLogLevel,
                PlayerId = playerId
            };

            _dbContext.Add(playerSettings);

            await _dbContext.SaveChangesAsync();

            return playerSettings;
        }


        private async Task<PlayerStatistics> CreatePlayerStatistics(Guid playerId)
        {
            var playerStatistics = new PlayerStatistics()
            {
                Id = Guid.NewGuid(),
                PlayerId = playerId
            };

            _dbContext.Add(playerStatistics);

            await _dbContext.SaveChangesAsync();

            return playerStatistics;
        }
    }
}
