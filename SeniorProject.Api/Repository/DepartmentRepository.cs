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
    public class DepartmentRepository : IRepository<DepartmentEntity>
    {
        private readonly ShoppingAssistantAPIContext _dbContext;

        public DepartmentRepository(ShoppingAssistantAPIContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<bool> AddEntityAsync(DepartmentEntity entity, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteEntity(int id, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<List<DepartmentEntity>> GetAllEntities(CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResults<DepartmentEntity>> GetEntitiesAsync(CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<DepartmentEntity> GetEntityAsync(int id, CancellationToken ct)
        {
            var department = await _dbContext.Departments.FirstOrDefaultAsync(d => d.Id == id);

            return department;
        }

        public Task<DepartmentEntity> UpdateEntity(DepartmentEntity entity, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
