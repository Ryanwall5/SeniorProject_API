using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SeniorProject.Api.Models.Entities;
using SeniorProject.Api.Models.FromApp;
using SeniorProject.Api.Models.Helper;

namespace SeniorProject.Api.Repository
{
    public class StoreUserRepository : IStoreUserRepository
    {

        private readonly ShoppingAssistantAPIContext _dbContext;

        public StoreUserRepository(ShoppingAssistantAPIContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<bool> AddEntityAsync(StoreUserEntity entity, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteEntity(int id, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<List<StoreUserEntity>> GetAllEntities(CancellationToken ct)
        {
            var storeUsers = await _dbContext.StoreUsers.ToListAsync();
            return storeUsers;
        }

        public Task<PagedResults<StoreUserEntity>> GetEntitiesAsync(CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<StoreUserEntity> GetEntityAsync(int id, CancellationToken ct)
        {
            var user = await _dbContext.StoreUsers.FirstAsync();
            return user;
        }

        public Task<StoreUserEntity> GetEntityAsync(string email, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<StoreUserEntity> UpdateEntity(StoreUserEntity entity, CancellationToken ct)
        {
            try
            {
                _dbContext.Entry(entity).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
                return entity;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public async Task<Tuple<bool, string>> AddEntityAsync(APIStoreUser entity, CancellationToken ct)
        {
            try
            {
                if (await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == entity.Email) == null)
                {
                    StoreUserEntity newUser = new StoreUserEntity()
                    {
                        Email = entity.Email,
                        UserName = entity.Email,
                        PasswordHash = entity.Password,
                        FirstName = entity.FirstName,
                        LastName = entity.LastName,
                        TimeOfCreation = DateTimeOffset.UtcNow
                    };

                    await _dbContext.StoreUsers.AddAsync(newUser);

                    await _dbContext.SaveChangesAsync();

                    return Tuple.Create(true, "");
                }
                else
                {
                    return Tuple.Create(false, $"Already have a user in database with that email {entity.Email}");
                }
            }
            catch (Exception ex)
            {
                return Tuple.Create(false, "Unable to add user"); ;
            }
        }

        public async Task<StoreUserEntity> GetEntityAsync(Guid id, CancellationToken ct)
        {
            return await _dbContext.StoreUsers.FirstOrDefaultAsync(su => su.Id == id);
        }
    }
}
