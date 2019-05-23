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
    public class ShelfRepository : IShelfRepository
    {
        private readonly ShoppingAssistantAPIContext _dbContext;

        public ShelfRepository(ShoppingAssistantAPIContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Task<bool> AddEntityAsync(ShelfEntity entity, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteEntity(int id, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public List<ShelfEntity> GetAllEntities(int sectionId, CancellationToken ct)
        {
            var shelves = _dbContext.Shelfs.Where(ld => ld.SectionId == sectionId).ToList();
            return shelves;
        }

        public Task<List<ShelfEntity>> GetAllEntities(CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResults<ShelfEntity>> GetEntitiesAsync(CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<ShelfEntity> GetEntityAsync(int id, CancellationToken ct)
        {
            var shelf = await _dbContext.Shelfs.FirstOrDefaultAsync(d => d.Id == id);

            return shelf;
        }

        public Task<ShelfEntity> UpdateEntity(ShelfEntity entity, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
