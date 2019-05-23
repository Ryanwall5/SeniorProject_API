using Microsoft.EntityFrameworkCore;
using SeniorProject.Api.Models.Entities;
using SeniorProject.Api.Models.FromApp;
using SeniorProject.Api.Models.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SeniorProject.Api.Repository
{
    public class ShoppingUserRepository : IShoppingUserRepository
    {

        private readonly ShoppingAssistantAPIContext _dbContext;

        public ShoppingUserRepository(ShoppingAssistantAPIContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Tuple<bool, string>> AddEntityAsync(APIShoppingUser entity)
        {
            try
            {
                var users = _dbContext.Users.ToList();
                if (users.FirstOrDefault(u => u.Email == entity.Email) == null)
                {
                    ShoppingUserEntity newUser = new ShoppingUserEntity()
                    {
                        Email = entity.Email,
                        UserName = entity.UserName,
                        PasswordHash = entity.Password,
                        FirstName = entity.FirstName,
                        LastName = entity.LastName,
                        HomeStoreId = entity.HomeStoreId,
                        TimeOfCreation = DateTimeOffset.UtcNow
                    };

                    await _dbContext.ShoppingUsers.AddAsync(newUser);
                    await _dbContext.SaveChangesAsync();
                    return Tuple.Create(true, "");
                }
                else
                {
                    return Tuple.Create(false, "Already have a user in database with that email");
                }

            }
            catch (Exception ex)
            {
                return Tuple.Create(false, "Unable to add user"); ;
            }
        }

        public Task<ShoppingUserEntity> AddEntityAsync(ShoppingUserEntity entity, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteEntity(int id, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ShoppingUserEntity>> GetAllEntities(CancellationToken ct)
        {
            var shoppingUsers = await _dbContext.ShoppingUsers.ToListAsync();
            return shoppingUsers;
        }

        public Task<PagedResults<ShoppingUserEntity>> GetEntitiesAsync(CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<ShoppingUserEntity> GetEntityAsync(int id, CancellationToken ct)
        {
            var user = await _dbContext.ShoppingUsers.FirstAsync();
            return user;
        }

        public async Task<ShoppingUserEntity> GetEntityAsync(string email, CancellationToken ct)
        {
            var userWithStore = await _dbContext.ShoppingUsers.FirstOrDefaultAsync(u => u.Email == email);
                //.FirstOrDefaultAsync(u => u.Email == email);

            if (userWithStore == null)
            {
                return null;
            }

            return userWithStore;
        }

        public async Task<ShoppingUserEntity> UpdateEntity(ShoppingUserEntity entity, CancellationToken ct)
        {
            _dbContext.ShoppingUsers.Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        Task<bool> IRepository<ShoppingUserEntity>.AddEntityAsync(ShoppingUserEntity entity, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<ShoppingUserEntity> GetEntityAsync(Guid id, CancellationToken ct)
        {
            return await _dbContext.ShoppingUsers.FirstOrDefaultAsync(su => su.Id == id);
        }
    }
}
