using Microsoft.EntityFrameworkCore;
using Nutricenter.ContractsBilling.Domain.Aggregate;
using Nutricenter.ContractsBilling.Domain.Interfaces.Repositories;
namespace Nutricenter.ContractsBilling.Infrastructure.Persistence.Repositories
{
    internal class ContractRepository(DomainDbContext dbContext): IContractRepository
    {
        private readonly DomainDbContext _dbContext = dbContext;

        public async Task<Contract?> GetByIdAsync(Guid id, bool readOnly = false)
        {
            if (readOnly)
            {
                return await _dbContext.Contracts
                    .AsNoTracking()
                    .Include(c => c.Services)
                    .FirstOrDefaultAsync(i => i.Id == id);
            }
            else
            {
                return await _dbContext.Contracts
                    .Include(c => c.Services)
                    .FirstOrDefaultAsync(i => i.Id == id);
            }
        }
        public Task CreateAsync(Contract entity)
        {
            _dbContext.Contracts.AddAsync(entity);
            return Task.CompletedTask; // UoW hará el SaveChanges
        }
        public Task UpdateAsync(Contract entity)
        {
            _dbContext.Contracts.Update(entity);
            return Task.CompletedTask; // UoW hará el SaveChanges
        }
        public Task DeleteAsync(Guid id)
        {
            var entity = _dbContext.Contracts.Find(id);
            if (entity != null)
            {
                _dbContext.Contracts.Remove(entity);
            }
            return Task.CompletedTask; // UoW hará el SaveChanges
        }
    }
}
