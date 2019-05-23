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
    public class StoreMapRepository : IRepository<StoreMapEntity>
    {
        private readonly ShoppingAssistantAPIContext _dbContext;

        public StoreMapRepository(ShoppingAssistantAPIContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<bool> AddEntityAsync(StoreMapEntity entity, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteEntity(int id, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<List<StoreMapEntity>> GetAllEntities(CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResults<StoreMapEntity>> GetEntitiesAsync(CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<StoreMapEntity> GetEntityAsync(int id, CancellationToken ct)
        {
            StoreMapEntity map = await _dbContext.StoreMaps.FirstOrDefaultAsync(sm => sm.Id == id);

            if(map == null)
            {
                return null;
            }

            var departments = _dbContext.Departments.Where(d => d.StoreMapId == map.Id).ToList();

            if(departments == null)
            {
                return map;
            }

            map.Departments = departments;

            return map;
        }

        public Task<StoreMapEntity> UpdateEntity(StoreMapEntity entity, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
