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
    public class StoreRepository : IRepository<StoreEntity>
    {

        private readonly ShoppingAssistantAPIContext _dbContext;

        public StoreRepository(ShoppingAssistantAPIContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AddEntityAsync(StoreEntity entity, CancellationToken ct)
        {
            try
            {
                StoreEntity store = await _dbContext.Stores.FirstOrDefaultAsync(s => s.PhoneNumber == entity.PhoneNumber && s.Name == entity.Name);

                if (store != null)
                {
                    throw new Exception("Store is already in database");
                }

                await _dbContext.Stores.AddAsync(entity);

                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteEntity(int id, CancellationToken ct)
        {
            var store = await _dbContext.Stores.FirstOrDefaultAsync(s => s.Id == id);
            if (store != null)
            {
                _dbContext.Stores.Remove(store);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<StoreEntity>> GetAllEntities(CancellationToken ct)
        {
            var stores = await _dbContext.Stores.ToListAsync();
            return stores;
        }

        public Task<StoreEntity> GetEntitiesAsync(CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<StoreEntity> GetEntityAsync(int id, CancellationToken ct)
        {
            return await _dbContext.Stores.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<StoreEntity> UpdateEntity(StoreEntity entity, CancellationToken ct)
        {
            try
            {
                _dbContext.Entry(entity).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        Task<PagedResults<StoreEntity>> IRepository<StoreEntity>.GetEntitiesAsync(CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
