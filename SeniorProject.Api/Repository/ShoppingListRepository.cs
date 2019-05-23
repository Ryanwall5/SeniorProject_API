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
    public class ShoppingListRepository : IShoppingListRepository
    {
        private readonly ShoppingAssistantAPIContext _dbContext;

        public ShoppingListRepository(ShoppingAssistantAPIContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Tuple<bool, string>> AddShoppingList(ShoppingListEntity entity, CancellationToken ct)
        {
            try
            {
                if (await _dbContext.ShoppingLists.FirstOrDefaultAsync(list => list.Name == entity.Name && list.StoreId == entity.StoreId) == null)
                {
                    await _dbContext.ShoppingLists.AddAsync(entity);
                    await _dbContext.SaveChangesAsync();
                    return new Tuple<bool, string>(true, $"Shopping list with name {entity.Name} was created at that store");
                }
                return new Tuple<bool, string>(false, $"Already have a list with name {entity.Name} at that store");
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string>(false, $"Error creating store");
            }
        }

        public async Task<List<ShoppingListEntity>> GetAllShoppingListsForUserForACertainStore(Guid userId, int storeId, CancellationToken ct)
        {
            return await _dbContext.ShoppingLists.Where(list => list.ShoppingUserId == userId && list.StoreId == storeId).ToListAsync();
        }

        public async Task<bool> DeleteEntity(int id, CancellationToken ct)
        {
            var shoppingListEntity = await _dbContext.ShoppingLists.FirstOrDefaultAsync(sl => sl.Id == id);
            if (shoppingListEntity != null)
            {
                _dbContext.ShoppingLists.Remove(shoppingListEntity);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public Task<List<ShoppingListEntity>> GetAllEntities(CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResults<ShoppingListEntity>> GetEntitiesAsync(CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<ShoppingListEntity> GetEntityAsync(int id, CancellationToken ct)
        {
            return await _dbContext.ShoppingLists.FirstOrDefaultAsync(sl => sl.Id == id);
        }

        public async Task<ShoppingListEntity> UpdateEntity(ShoppingListEntity entity, CancellationToken ct)
        {
            _dbContext.ShoppingLists.Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<List<ShoppingListEntity>> GetAllShoppingListsForUser(Guid userId, CancellationToken ct)
        {
            return await _dbContext.ShoppingLists.Where(list => list.ShoppingUserId == userId).ToListAsync();
        }

        public Task<bool> AddEntityAsync(ShoppingListEntity entity, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
