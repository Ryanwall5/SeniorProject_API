using Microsoft.EntityFrameworkCore;
using SeniorProject.Api.Models.Entities;
using SeniorProject.Api.Models.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SeniorProject.Api.Repository
{
    public class BaseUserRepository : IRepository<BaseUserEntity>
    {
        private readonly ShoppingAssistantAPIContext _dbContext;

        public BaseUserRepository(ShoppingAssistantAPIContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<bool> AddEntityAsync(BaseUserEntity entity, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteEntity(int id, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<List<BaseUserEntity>> GetAllEntities(CancellationToken ct)
        {
            var users = await _dbContext.Users.ToListAsync();
            return users;
        }

        public Task<PagedResults<BaseUserEntity>> GetEntitiesAsync(CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<BaseUserEntity> GetEntityAsync(int id, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<BaseUserEntity> UpdateEntity(BaseUserEntity entity, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

    }
}
