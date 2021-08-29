using AutoMapper;
using EvolutionOfCreatures.Db;
using EvolutionOfCreatures.Db.Entities;
using EvolutionOfCreatures.Logic.Players;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EvolutionOfCreatures.Logic.Accounts
{
    public class AccountManager : IAccountManager
    {
        private readonly IValidator<CreateAccountRequest> _validatorCreateRequest;
        private readonly IPlayerManager _playerManager;
        private readonly EvolutionOfCreaturesContext _dbContext;
        private readonly IMapper _mapper;


        public AccountManager(IPlayerManager playerManager,
                              EvolutionOfCreaturesContext dbContext,
                              IValidator<CreateAccountRequest> validatorCreateRequest)
        {
            _playerManager = playerManager;
            _dbContext = dbContext;
            _validatorCreateRequest = validatorCreateRequest;
        }


        public async Task<AccountDto> Create(CreateAccountRequest request)
        {
            await _validatorCreateRequest.ValidateAndThrowAsync(request);

            var entity = new Account
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                TransactionIds = new List<string>()
            };

            await _dbContext.SaveChangesAsync();

            request.Player.AccountId = entity.Id;

            var player = await _playerManager.CreateEntity(request.Player);

            entity.Player = player;

            _dbContext.Add(entity);

            return _mapper.Map<AccountDto>(entity);
        }
    }
}
