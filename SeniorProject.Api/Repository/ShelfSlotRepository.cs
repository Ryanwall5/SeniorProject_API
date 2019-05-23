using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SeniorProject.Api.Models.Entities;
using SeniorProject.Api.Models.Helper;

namespace SeniorProject.Api.Repository
{
    public class ShelfSlotRepository : IShelfSlotsRepository
    { 
        private readonly ShoppingAssistantAPIContext _dbContext;

        public ShelfSlotRepository(ShoppingAssistantAPIContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<bool> AddEntityAsync(ShelfSlotEntity entity, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteEntity(int id, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public List<ShelfSlotEntity> GetAllEntities(int shelfId, CancellationToken ct)
        {
            var shelfSlots = _dbContext.ShelfSlots.Where(ld => ld.ShelfId == shelfId).ToList();
            return shelfSlots;
        }

        public Task<List<ShelfSlotEntity>> GetAllEntities(CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResults<ShelfSlotEntity>> GetEntitiesAsync(CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<ShelfSlotEntity> GetEntityAsync(int id, CancellationToken ct)
        {
            var shelfSlot = await _dbContext.ShelfSlots.FirstOrDefaultAsync(ld => ld.Id == id);
            return shelfSlot;
        }

        public Task<ShelfSlotEntity> UpdateEntity(ShelfSlotEntity entity, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
