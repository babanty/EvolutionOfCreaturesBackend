using Microsoft.VisualStudio.TestTools.UnitTesting;
using EvolutionOfCreatures.Logic.Players;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EvolutionOfCreatures.Db;
using EvolutionOfCreatures.Logic.Tests.TestTools;

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
            using var msSqlDbWrapper = GetNewMsSqlDbWrapper();
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



        private PlayerManager GetNewPlayerManager(EvolutionOfCreaturesContext db) => new PlayerManager(db, new CreatePlayerRequestValidator());


        private MsSqlDbContextWrapper<EvolutionOfCreaturesContext> GetNewMsSqlDbWrapper()
        {
            var dbContext = DbContextUtilities.GetMsSqlTestDb<EvolutionOfCreaturesContext>();

            return new MsSqlDbContextWrapper<EvolutionOfCreaturesContext>() { Instance = dbContext };
        }
    }
}