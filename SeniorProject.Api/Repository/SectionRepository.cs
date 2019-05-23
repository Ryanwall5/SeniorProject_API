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
    public class SectionRepository : ISectionRepository
    {

        private readonly ShoppingAssistantAPIContext _dbContext;

        public SectionRepository(ShoppingAssistantAPIContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<bool> AddEntityAsync(SectionEntity entity, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteEntity(int id, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<List<SectionEntity>> GetAllEntities(CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public List<SectionEntity> GetAllEntities(int aisleId, CancellationToken ct)
        {
            var sections = _dbContext.Sections.Where(ld => ld.AisleId == aisleId).ToList();
            return sections;
        }

        public Task<PagedResults<SectionEntity>> GetEntitiesAsync(CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<SectionEntity> GetEntityAsync(int id, CancellationToken ct)
        {
            var section = await _dbContext.Sections.FirstOrDefaultAsync(d => d.Id == id);

            return section;
        }

        public Task<SectionEntity> UpdateEntity(SectionEntity entity, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
