using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SeniorProject.Api.Models;
using SeniorProject.Api.Models.Entities;
using SeniorProject.Api.Models.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SeniorProject.Api.Repository
{
    public class ItemRepository : IItemEntityRepository
    {
        private readonly ShoppingAssistantAPIContext _dbContext;

        public ItemRepository(ShoppingAssistantAPIContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AddEntityAsync(ItemEntity entity, CancellationToken ct)
        {
            try
            {
                var item = await _dbContext.Items.FirstOrDefaultAsync(i => i.Name == entity.Name);
                if (item != null)
                {
                    return false;
                }

                await _dbContext.Items.AddAsync(entity);

                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Task<bool> DeleteEntity(int id, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ItemEntity>> GetAllEntities(CancellationToken ct)
        {
            var items = await _dbContext.Items.ToListAsync();
            return items;
        }

        public async Task<PagedResults<ItemEntity>> GetEntitiesAsync(CancellationToken ct)
        {
            IQueryable<ItemEntity> itemsQuery = _dbContext.Items;

            int size = await itemsQuery.CountAsync(ct);

            var items = await itemsQuery.Take(10).ToArrayAsync();

            return new PagedResults<ItemEntity>
            {
                Entities = items,
                Size = size
            };
        }

        public async Task<ItemEntity> GetEntityAsync(int id, CancellationToken ct)
        {
            var item = await _dbContext.Items.SingleOrDefaultAsync(i => i.Id == id, ct);

            if (item == null)
            {
                return null;
            }

            return item;
        }

        public async Task<ItemEntity> GetEntityBySpoonIdAsync(int id, CancellationToken ct)
        {
            var item = await _dbContext.Items.SingleOrDefaultAsync(i => i.SpoonacularProductId == id, ct);

            if (item == null)
            {
                return null;
            }

            return item;
        }


        public async Task<ItemEntity> UpdateEntity(ItemEntity entity, CancellationToken ct)
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
    }
}
