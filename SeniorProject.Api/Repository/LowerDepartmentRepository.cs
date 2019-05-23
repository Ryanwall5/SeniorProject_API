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
    public class LowerDepartmentRepository : ILowerDepartmentRepository
    {

        private readonly ShoppingAssistantAPIContext _dbContext;

        public LowerDepartmentRepository(ShoppingAssistantAPIContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<bool> AddEntityAsync(LowerDepartmentEntity entity, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteEntity(int id, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public List<LowerDepartmentEntity> GetAllEntities(int id, CancellationToken ct)
        {
            var LowerDepartments = _dbContext.LowerDepartments.Where(ld => ld.DepartmentId == id).ToList();

            return LowerDepartments;
        }

        public Task<List<LowerDepartmentEntity>> GetAllEntities(CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResults<LowerDepartmentEntity>> GetEntitiesAsync(CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<LowerDepartmentEntity> GetEntityAsync(int id, CancellationToken ct)
        {
            var LowerDepartment = await _dbContext.LowerDepartments.FirstOrDefaultAsync(ld => ld.Id == id);
            return LowerDepartment;
        }

        public Task<LowerDepartmentEntity> UpdateEntity(LowerDepartmentEntity entity, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
