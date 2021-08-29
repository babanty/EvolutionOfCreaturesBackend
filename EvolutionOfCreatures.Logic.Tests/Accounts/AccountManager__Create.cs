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
            var createAccountRequest = new CreateAccountRequest()
            {
                Player = new CreatePlayerRequest() { PlayerName = "stub" }
            };
            var entityManager = GetNewAccountManager(createAccountRequest.Player);

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


        private IAccountManager GetNewAccountManager(CreatePlayerRequest createPlayerRequest)
        {
            return new AccountManager(GetPlayerManagerMock(createPlayerRequest),
                                      GetNewDb(),
                                      new CreateAccountRequestValidator(),
                                      GetMapper());
        }


        [TestMethod()]
        public async Task Create__InvalidCreateRequestWithoutPlayer__ValidationException()
        {
            // Arrange
            var createAccountRequest = new CreateAccountRequest()
            {
                Player = null
            };
            var entityManager = GetNewAccountManager(createAccountRequest.Player);

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
            var entityManager = GetNewAccountManager(null);

            // Act
            async Task action() => await entityManager.Create(createAccountRequest);

            // Assert
            await Assert.ThrowsExceptionAsync<ValidationException>(action);
        }


        private EvolutionOfCreaturesContext GetNewDb() =>
            new EvolutionOfCreaturesContext(DbContextUtilities.GetInMemoryDbOptions<EvolutionOfCreaturesContext>());


        private IPlayerManager GetPlayerManagerMock(CreatePlayerRequest createPlayerRequest)
        {
            var mock = new Mock<IPlayerManager>();

            if (createPlayerRequest is null)
            {
                mock.Setup(a => a.CreateEntity(null))
                                 .Returns(Task.FromResult((Player)null));
            }
            else
            {
                mock.Setup(a => a.CreateEntity(createPlayerRequest))
                                 .Returns(Task.FromResult(
                                             new Player()
                                             {
                                                 AccountId = createPlayerRequest.AccountId,
                                                 PlayerProgress = new PlayerProgress(),
                                                 PlayerSettings = new PlayerSettings(),
                                                 PlayerStatistics = new PlayerStatistics()
                                             }));
            }

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