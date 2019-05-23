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
    public class AdminUserRepository : IRepository<AdminUserEntity>
    {
        private readonly ShoppingAssistantAPIContext _dbContext;

        public AdminUserRepository(ShoppingAssistantAPIContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<bool> AddEntityAsync(AdminUserEntity entity, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteEntity(int id, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<List<AdminUserEntity>> GetAllEntities(CancellationToken ct)
        {
            var admins = await _dbContext.AdminUsers.ToListAsync();
            return admins;
        }

        public Task<PagedResults<AdminUserEntity>> GetEntitiesAsync(CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<AdminUserEntity> GetEntityAsync(int id, CancellationToken ct)
        {
            var user = await _dbContext.AdminUsers.FirstAsync();
            return user;
        }

        public Task<AdminUserEntity> UpdateEntity(AdminUserEntity entity, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
