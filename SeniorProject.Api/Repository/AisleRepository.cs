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
    public class AisleRepository : IAisleRepository
    {

        private readonly ShoppingAssistantAPIContext _dbContext;

        public AisleRepository(ShoppingAssistantAPIContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<bool> AddEntityAsync(AisleEntity entity, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteEntity(int id, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<List<AisleEntity>> GetAllEntities(CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public List<AisleEntity> GetAllEntities(int lowerDepartmentId, CancellationToken ct)
        {
            var aisles = _dbContext.Aisles.Where(ld => ld.LowerDepartmenttId == lowerDepartmentId).ToList();
            return aisles;
        }

        public Task<PagedResults<AisleEntity>> GetEntitiesAsync(CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<AisleEntity> GetEntityAsync(int id, CancellationToken ct)
        {
            var aisle = await _dbContext.Aisles.FirstOrDefaultAsync(ld => ld.Id == id);
            return aisle;
        }

        public Task<AisleEntity> UpdateEntity(AisleEntity entity, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
