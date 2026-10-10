using Microsoft.EntityFrameworkCore;
using Nutricenter.ContractsBilling.Domain.Entities;
using Nutricenter.ContractsBilling.Domain.Interfaces.Repositories;

namespace Nutricenter.ContractsBilling.Infrastructure.Persistence.Repositories
{
    internal class ServiceTypeRepository(DomainDbContext dbContext) : IServiceTypeRepository
    {
        private readonly DomainDbContext _dbContext = dbContext;

        public async Task<ServiceType?> GetByIdAsync(Guid id, bool readOnly = false)
        {
            if (readOnly)
            {
                return await _dbContext.ServiceTypes.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
            }
            else
            {
                return await _dbContext.ServiceTypes.FindAsync(id);
            }
        }
        public Task CreateAsync(ServiceType entity)
        {
            _dbContext.ServiceTypes.AddAsync(entity);
            return Task.CompletedTask; // UoW hará el SaveChanges
        }
        public Task UpdateAsync(ServiceType entity)
        {
            _dbContext.ServiceTypes.Update(entity);
            return Task.CompletedTask; // UoW hará el SaveChanges
        }
        public Task DeleteAsync(Guid id)
        {
            var entity = _dbContext.ServiceTypes.Find(id);
            if (entity != null)
            {
                _dbContext.ServiceTypes.Remove(entity);
            }
            return Task.CompletedTask; // UoW hará el SaveChanges
        }
    }
}
