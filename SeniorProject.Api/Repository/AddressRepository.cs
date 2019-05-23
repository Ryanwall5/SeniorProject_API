

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SeniorProject.Api.Models.Entities;
using SeniorProject.Api.Models.Helper;

namespace SeniorProject.Api.Repository
{
    public class AddressRepository : IRepository<AddressEntity>
    {
        private readonly ShoppingAssistantAPIContext _dbContext;

        public AddressRepository(ShoppingAssistantAPIContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AddEntityAsync(AddressEntity entity, CancellationToken ct)
        {
            try
            {
                AddressEntity address = await _dbContext.Addresses.FirstOrDefaultAsync(a => a.Latitude == entity.Latitude
                                                                                && a.Longitude == entity.Longitude
                                                                                && a.Street == entity.Street);
                if(address != null)
                {
                    throw new Exception("Address already in database");
                }

                await _dbContext.Addresses.AddAsync(entity);

                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteEntity(int id, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<List<AddressEntity>> GetAllEntities(CancellationToken ct)
        {
            var addresses = await _dbContext.Addresses.ToListAsync();
            return addresses;
        }

        public Task<PagedResults<AddressEntity>> GetEntitiesAsync(CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<AddressEntity> GetEntityAsync(int id, CancellationToken ct)
        {
            return await _dbContext.Addresses.FirstOrDefaultAsync(address => address.Id == id);
        }

        public Task<AddressEntity> UpdateEntity(AddressEntity entity, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
