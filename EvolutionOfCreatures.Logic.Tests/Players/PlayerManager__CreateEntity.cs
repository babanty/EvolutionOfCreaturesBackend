using Microsoft.VisualStudio.TestTools.UnitTesting;
using EvolutionOfCreatures.Logic.Players;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EvolutionOfCreatures.Db;
using EvolutionOfCreatures.Logic.Tests.TestTools;
using EvolutionOfCreatures.Db.Entities;
using FluentValidation;

namespace EvolutionOfCreatures.Logic.Players.Tests
{
    [TestClass()]
    public class PlayerManager__CreateEntity
    {
        [TestMethod()]
        public async Task CreateEntity__ValidCreateRequest__ResultOk()
        {
            // Arrange
            var accountId = Guid.NewGuid();
            var playerName = "stub";
            var createPlayerRequest = new CreatePlayerRequest() { AccountId = accountId, PlayerName = playerName };
            using var msSqlDbWrapper = GetNewMsSqlDbWrapper(accountId);
            var entityManager = GetNewPlayerManager(msSqlDbWrapper.Instance);

            // Act
            var newEntity = await entityManager.CreateEntity(createPlayerRequest);

            // Assert
            Assert.AreNotEqual(Guid.Empty, newEntity.Id);
            Assert.AreNotEqual(default, newEntity.CreatedAt);
            Assert.IsNotNull(newEntity.PlayerProgress);
            Assert.IsNotNull(newEntity.PlayerSettings);
            Assert.IsNotNull(newEntity.PlayerStatistics);
            Assert.AreEqual(accountId, newEntity.AccountId);
            Assert.AreEqual(playerName, newEntity.Name);
            Assert.AreNotEqual(default, newEntity.Rating); // business case. Rating cannot be empty, because other players need to take the rating when they win.
        }


        [TestMethod()]
        public async Task Create__CreateRequestIsNull__ValidationException()
        {
            // Arrange
            CreatePlayerRequest createPlayerRequest = null;
            var entityManager = GetNewPlayerManager(GetNewInMemoryDb());

            // Act
            async Task action() => await entityManager.CreateEntity(createPlayerRequest);

            // Assert
            await Assert.ThrowsExceptionAsync<ValidationException>(action);
        }


        [TestMethod()]
        public async Task Create__InvalidCreateRequestWithoutName__ValidationException()
        {
            // Arrange
            var createPlayerRequest = new CreatePlayerRequest()
            {
                AccountId = Guid.NewGuid(),
                PlayerName = null
            };
            var entityManager = GetNewPlayerManager(GetNewInMemoryDb());

            // Act
            async Task action() => await entityManager.CreateEntity(createPlayerRequest);

            // Assert
            await Assert.ThrowsExceptionAsync<ValidationException>(action);
        }


        [TestMethod()]
        public async Task Create__InvalidCreateRequestWithInvalidName__ValidationException()
        {
            // Arrange
            var createPlayerRequest = new CreatePlayerRequest()
            {
                AccountId = Guid.NewGuid(),
                PlayerName = "a"
            };
            var entityManager = GetNewPlayerManager(GetNewInMemoryDb());

            // Act
            async Task action() => await entityManager.CreateEntity(createPlayerRequest);

            // Assert
            await Assert.ThrowsExceptionAsync<ValidationException>(action);
        }


        [TestMethod()]
        public async Task Create__InvalidCreateRequestWithoutAccountId__ValidationException()
        {
            // Arrange
            var createPlayerRequest = new CreatePlayerRequest()
            {
                AccountId = Guid.Empty,
                PlayerName = "stub"
            };
            var entityManager = GetNewPlayerManager(GetNewInMemoryDb());

            // Act
            async Task action() => await entityManager.CreateEntity(createPlayerRequest);

            // Assert
            await Assert.ThrowsExceptionAsync<ValidationException>(action);
        }



        private PlayerManager GetNewPlayerManager(EvolutionOfCreaturesContext db) => new PlayerManager(db, new CreatePlayerRequestValidator());


        private MsSqlDbContextWrapper<EvolutionOfCreaturesContext> GetNewMsSqlDbWrapper(Guid accountId)
        {
            var dbContext = DbContextUtilities.GetMsSqlTestDb<EvolutionOfCreaturesContext>();

            DbDataInitialization(dbContext, accountId);

            return new MsSqlDbContextWrapper<EvolutionOfCreaturesContext>() { Instance = dbContext };
        }


        private EvolutionOfCreaturesContext GetNewInMemoryDb() => DbContextUtilities.GetEfCoreInMemoryDb<EvolutionOfCreaturesContext>();


        private void DbDataInitialization(EvolutionOfCreaturesContext dbContext, Guid accountId)
        {
            dbContext.Add(new Account() { Id = accountId });
            dbContext.SaveChanges();
        }
    }
}