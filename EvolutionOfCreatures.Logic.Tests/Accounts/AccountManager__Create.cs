using EvolutionOfCreatures.Logic.Accounts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using EvolutionOfCreatures.Logic.Tests.TestTools;
using EvolutionOfCreatures.Db;
using EvolutionOfCreatures.Logic.Players;
using Moq;
using System.Threading.Tasks;
using EvolutionOfCreatures.Db.Entities;
using AutoMapper;
using Infrastructure.Tools.Exceptions;
using FluentValidation;

namespace EvolutionOfCreatures.Logic.Accounts.Tests
{
    [TestClass()]
    public class AccountManager__Create
    {

        [TestMethod()]
        public async Task Create__ValidCreateRequest__ResultOk()
        {
            // Arrange
            var createAccountRequest = new CreateAccountRequest() { Name = "stub" };
            var entityManager = GetNewAccountManager();

            // Act
            var newEntity = await entityManager.Create(createAccountRequest);

            // Assert
            Assert.AreNotEqual(Guid.Empty, newEntity.Id);
            Assert.AreNotEqual(new DateTime(), newEntity.CreatedAt);
            Assert.IsNotNull(newEntity.Player);
            Assert.IsNotNull(newEntity.Player?.PlayerProgress);
            Assert.IsNotNull(newEntity.Player?.PlayerSettings);
            Assert.IsNotNull(newEntity.Player?.PlayerStatistics);
        }


        [TestMethod()]
        public async Task Create__InvalidCreateRequestWithoutName__ValidationException()
        {
            // Arrange
            var createAccountRequest = new CreateAccountRequest() { Name = null };
            var entityManager = GetNewAccountManager();

            // Act
            async Task action() => await entityManager.Create(createAccountRequest);

            // Assert
            await Assert.ThrowsExceptionAsync<ValidationException>(action);
        }


        [TestMethod()]
        public async Task Create__CreateRequestIsNull__ValidationException()
        {
            // Arrange
            CreateAccountRequest createAccountRequest = null;
            var entityManager = GetNewAccountManager();

            // Act
            async Task action() => await entityManager.Create(createAccountRequest);

            // Assert
            await Assert.ThrowsExceptionAsync<ValidationException>(action);
        }


        private IAccountManager GetNewAccountManager()
        {
            return new AccountManager(GetPlayerManagerMock(),
                                      GetNewDb(),
                                      new CreateAccountRequestValidator(),
                                      GetMapper());
        }


        private EvolutionOfCreaturesContext GetNewDb() =>
            new EvolutionOfCreaturesContext(DbContextUtilities.GetInMemoryDbOptions<EvolutionOfCreaturesContext>());


        private IPlayerManager GetPlayerManagerMock()
        {
            var mock = new Mock<IPlayerManager>();

            mock.Setup(a => a.CreateEntity(It.IsAny<CreatePlayerRequest>()))
                                 .Returns(Task.FromResult(
                                             new Player()
                                             {
                                                 PlayerProgress = new PlayerProgress(),
                                                 PlayerSettings = new PlayerSettings(),
                                                 PlayerStatistics = new PlayerStatistics()
                                             }));

            return mock.Object;
        }


        private IMapper GetMapper()
        {
            return new Mapper(new MapperConfiguration(c =>
            {
                c.AddProfile(new AccountMappingProfile());
                c.AddProfile(new PlayerMappingProfile());
            }));
        }
    }
}