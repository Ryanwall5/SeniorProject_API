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
    public class ItemStoreLinkRepository : IItemStoreLinkRepository
    {
        private readonly ShoppingAssistantAPIContext _dbContext;

        public ItemStoreLinkRepository(ShoppingAssistantAPIContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AddEntityAsync(ItemStoreLinkEntity entity, CancellationToken ct)
        {

            try
            {
                await _dbContext.ItemStoreLinks.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> DeleteEntity(int id, CancellationToken ct)
        {
            //try
            //{
            //    ItemStoreLinkEntity itemStoreLink = await _dbContext.ItemStoreLinks.FirstOrDefaultAsync(link => link.Id == id);
            //    if (itemStoreLink != null)
            //    {
            //        _dbContext.ItemStoreLinks.Remove(itemStoreLink);
            //        await _dbContext.SaveChangesAsync();
            //        return true;
            //    }
            //    return false;
            //}
            //catch (Exception ex)
            //{
            //    return false;
            //}
            return false;
        }

        public async Task<List<ItemStoreLinkEntity>> GetAllEntities(int storeId, CancellationToken ct)
        {
            try
            {
                var itemStoreLinks = await _dbContext.ItemStoreLinks
                    .Where(link => link.StoreId == storeId)
                    .Include(isl => isl.Item)
                    .Include(s => s.Store)
                    .ToListAsync();
                return itemStoreLinks;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Task<List<ItemStoreLinkEntity>> GetAllEntities(CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResults<ItemStoreLinkEntity>> GetEntitiesAsync(int storeId, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResults<ItemStoreLinkEntity>> GetEntitiesAsync(CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<ItemStoreLinkEntity> GetEntityAsync(int itemId, int storeId, CancellationToken ct)
        {
            try
            {
                ItemStoreLinkEntity itemStoreLink = await _dbContext.ItemStoreLinks.FirstOrDefaultAsync(link => link.ItemId == itemId && link.StoreId == storeId);

                if (itemStoreLink != null)
                {
                    return itemStoreLink;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ItemStoreLinkEntity> GetEntityAsync(int linkId, CancellationToken ct)
        {
            try
            {
                ItemStoreLinkEntity itemStoreLink = await _dbContext.ItemStoreLinks.FirstOrDefaultAsync(link => link.Id == linkId);

                if (itemStoreLink != null)
                {
                    return itemStoreLink;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ItemStoreLinkEntity> UpdateEntity(ItemStoreLinkEntity entity, CancellationToken ct)
        {
            try
            {
                _dbContext.ItemStoreLinks.Update(entity);
                await _dbContext.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
