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
    public class ItemShoppingListLinkRepository : IItemShoppingListLinkRepository
    {

        private readonly ShoppingAssistantAPIContext _dbContext;

        public ItemShoppingListLinkRepository(ShoppingAssistantAPIContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<bool> AddEntityAsync(ItemShoppingListLinkEntity entity, CancellationToken ct)
        {
            try
            {
                ItemShoppingListLinkEntity islink = await _dbContext.ItemShoppingListLinks.FirstOrDefaultAsync(il => il.ItemId == entity.ItemId && il.ShoppingListId == entity.ShoppingListId);

                if (islink != null)
                {
                    throw new Exception("Already have that item in your shopping list");
                }

                await _dbContext.ItemShoppingListLinks.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<ItemShoppingListLinkEntity> AddNewLink(ItemShoppingListLinkEntity entity, CancellationToken ct)
        {
            try
            {
                ItemShoppingListLinkEntity islink = await _dbContext.ItemShoppingListLinks.FirstOrDefaultAsync(il => il.ItemId == entity.ItemId && il.ShoppingListId == entity.ShoppingListId);

                if (islink != null)
                {
                    throw new Exception("Already have that item in your shopping list");
                }

                await _dbContext.ItemShoppingListLinks.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<bool> DeleteEntity(int id, CancellationToken ct)
        {
            try
            {
                ItemShoppingListLinkEntity islink = await _dbContext.ItemShoppingListLinks.FirstOrDefaultAsync(il => il.Id == id);

                if (islink == null)
                {
                    throw new Exception("Item Shopping List Link Does not Exist");
                }

                _dbContext.ItemShoppingListLinks.Remove(islink);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<List<ItemShoppingListLinkEntity>> GetAllByShoppingListId(int shoppingListId, CancellationToken ct)
        {
            return await _dbContext.ItemShoppingListLinks.Where(link => link.ShoppingListId == shoppingListId).ToListAsync();
        }

        public Task<List<ItemShoppingListLinkEntity>> GetAllEntities(CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResults<ItemShoppingListLinkEntity>> GetEntitiesAsync(CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<ItemShoppingListLinkEntity> GetEntityAsync(int id, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<ItemShoppingListLinkEntity> GetShoppingItem(int itemShoppingListLinkId, CancellationToken ct)
        {
            var item = await _dbContext.ItemShoppingListLinks.FirstOrDefaultAsync(link => link.Id == itemShoppingListLinkId);
            return item;
        }

        public async Task<ItemShoppingListLinkEntity> UpdateEntity(ItemShoppingListLinkEntity entity, CancellationToken ct)
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

        Task<bool> IRepository<ItemShoppingListLinkEntity>.AddEntityAsync(ItemShoppingListLinkEntity entity, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
